using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Node.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Node.InternalServices
{
    public class NodeProcess : INodeProcess
    {
        private class ExecutionArg
        {
            //public bool UseDebugger { get; set; }
            public CancellationToken CancellationToken { get; set; }
            public string Arguments { get; set; }
        }

        public NodeProcess(NodeProcessFactory nodeProcessFactory, NodeContext nodeContext)
        {
            NodeProcessFactory = nodeProcessFactory;
            NodeContext = nodeContext;
            StdOut = new StringBuilder();
            StdErr = new StringBuilder();
            ResponseMutex = new SemaphoreSlim(0);
            Responses = new List<NodeResponse>();
        }
        private NodeProcessFactory NodeProcessFactory { get; }
        private NodeContext NodeContext { get; }
        internal string NodeWorkingDir => NodeContext.NodeWorkingDir;
        internal Guid Scope => NodeContext.Scope;
        internal ILogger Logger => NodeProcessFactory.Logger;
        internal ISerializerFactory SerializerFactory => NodeProcessFactory.SerializerFactory;

        private SemaphoreSlim ResponseMutex { get; }
        private StringBuilder StdErr { get; }
        private StringBuilder StdOut { get; }
        private Process Process { get; set; }
        private IList<NodeResponse> Responses { get; set; }

        public void Dispose()
        {
            NodeProcessFactory.ReturnNodeProcess(this);
        }

        public async Task<bool> IsAlive(CancellationToken cancellationToken)
        {
            try
            {
                return await ExecuteScriptAsync<bool>("true", cancellationToken);
            } 
            catch
            {
                return false;
            }
        }

        public async Task Kill()
        {
            if (Process == null) return;
            while(!Process.HasExited)
            {
                Process.Kill();
                await Task.Delay(100);
            }
            Process.Close();
            Process.Dispose();
        }


        public async Task<NodeExecution> ExecuteScriptAsync(string js, CancellationToken cancellationToken = default)
        {
            if (Process == null)
            {
                var scriptFileName = Path.Combine(NodeContext.NodeWorkingDir, $"script.js");
                using (var sw = File.Open(scriptFileName, FileMode.OpenOrCreate, FileAccess.Write))
                using (var tw = new StreamWriter(sw))
                {
                    tw.Write(@"
let id = 'dummy';
function solidRpcSendRespone(resp) {
    console.log(JSON.stringify({Id: id, Result: JSON.stringify(resp)}));
}
function handleCommand(chunk) {
    var x = JSON.parse(chunk);
    id = x.Id;
    var r = eval(x.Script);
    if(typeof(r) === 'object') {
       r.then(o => {
           solidRpcSendRespone(o);
       });
    } else {
       solidRpcSendRespone(r);
    }
}
process.stdin.resume();
process.stdin.setEncoding('utf8');
process.stdin.on('data', handleCommand);
");
                }


                Process = await Task.Factory.StartNew(StartProcess, new ExecutionArg()
                {
                    Arguments = scriptFileName,
                    CancellationToken = cancellationToken,
                }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            var command = new NodeCommand()
            {
                Id = Guid.NewGuid().ToString(),
                Script = js
            };
            string str;
            SerializerFactory.SerializeToString(out str, command);
            Process.StandardInput.WriteLine(str);
            await Process.StandardInput.FlushAsync();

            return await WaitForResponse(command.Id, cancellationToken);
        }

        private async Task<NodeExecution> WaitForResponse(string id, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    if (Process.HasExited)
                    {
                        return new NodeExecution()
                        {
                            ExitCode = Process.ExitCode,
                            Result = null,
                            Err = StdErr.ToString(),
                            Out = StdOut.ToString()
                        };
                    }
                    await ResponseMutex.WaitAsync(cancellationToken);
                    var resp = Responses.FirstOrDefault(o => o.Id == id);
                    if(resp != null)
                    {
                        return new NodeExecution()
                        {
                            ExitCode = Process.HasExited ? Process.ExitCode : 0,
                            Result = resp.Result,
                            Err = StdErr.ToString(),
                            Out = StdOut.ToString()
                        };
                    }
                }
            }
            finally
            {
                StdErr.Clear();
                StdOut.Clear();
            }
        }

        public async Task<T> ExecuteScriptAsync<T>(string js, CancellationToken cancellationToken = default)
        {
            var ne = await ExecuteScriptAsync(js, cancellationToken);
            if (ne.Result == null) return default(T);
            T retVal;
            SerializerFactory.DeserializeFromString(ne.Result, out retVal);
            return retVal;
        }

        public async Task<string> GetVersionAsync(CancellationToken cancellationToken)
        {
            var res = await Task.Factory.StartNew(RunJs, new ExecutionArg()
            {
                Arguments = $"--version"
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            if (res.ExitCode != 0)
            {
                throw new Exception($"Failed to get version - {res.Err}");
            }
            return res.Out;
        }

        public async Task<NodeExecution> ExecuteFileAsync(string fileName, IEnumerable<string> args, CancellationToken cancellationToken = default)
        {
            var scriptFile = Path.Combine(NodeContext.NodeModulesDir, fileName);
            return await Task.Factory.StartNew(RunJs, new ExecutionArg()
            {
                Arguments = $"{scriptFile} {string.Join(" ", args)}"
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private Process StartProcess(object oArg)
        {
            var eArg = (ExecutionArg)oArg;
            var nodeArgs = eArg.Arguments;

            //
            // save script to file
            //

            //
            //
            //
            var nodePath = Environment.GetEnvironmentVariable("NODE_PATH");
            var process = new Process();
            process.StartInfo.FileName = NodeContext.NodeExePath;
            process.StartInfo.Arguments = nodeArgs.Trim();
            process.StartInfo.WorkingDirectory = NodeContext.NodeWorkingDir;
            process.StartInfo.EnvironmentVariables["NODE_PATH"] = NodeContext.NodeModulesDir;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += OutputDataReceived;
            process.ErrorDataReceived += ErrorDataReceived;
            Logger.LogInformation($"Starting new node process({process.StartInfo.FileName}@{process.StartInfo.WorkingDirectory})...");
            if (!process.Start())
            {
                Logger.LogInformation("...failed to start new node process");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs data)
        {
            if (!string.IsNullOrEmpty(data.Data))
            {
                StdErr.AppendLine(data.Data);
            }
            ResponseMutex.Release();
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs data)
        {

            if (!string.IsNullOrEmpty(data.Data))
            {
                if(data.Data.StartsWith("{") && data.Data.EndsWith("}"))
                {
                    NodeResponse resp;
                    SerializerFactory.DeserializeFromString(data.Data, out resp);
                    if(resp?.Id != null)
                    {
                        Responses.Add(resp);
                        ResponseMutex.Release();
                        return;
                    }
                }
                StdOut.AppendLine(data.Data);
            }
        }

        private NodeExecution RunJs(object oArg)
        {
            var eArg = (ExecutionArg)oArg;
            try
            {
                int? exitCode = null;
                Process = StartProcess(oArg);
                try
                {

                    // connect debugger
                    var cancellationToken = eArg.CancellationToken;

                    // wait for script to complete
                    while (exitCode == null && !cancellationToken.IsCancellationRequested)
                    {
                        if (Process.WaitForExit(100))
                        {
                            exitCode = Process.ExitCode;
                        }
                    }
                }
                finally
                {
                    if (exitCode == null)
                    {
                        Logger.LogInformation($"Killing node process since no exit code returned.");
                        Process.Kill();
                    }
                    Process.Close();
                    Process.Dispose();
                    Process = null;
                }

                return new NodeExecution()
                {
                    ExitCode = exitCode ?? -1,
                    Out = StdOut.ToString(),
                    Err = StdErr.ToString()
                };
            }
            catch (Exception e)
            {
                return new NodeExecution()
                {
                    ExitCode = -1,
                    Err = e.Message,
                    Out = ""
                };
            }
        }



        //private NodeDebugger WaitForDebugger(Process process, StringBuilder stdErr, CancellationToken cancellationToken)
        //{
        //    //
        //    // grab debug port from std.err
        //    //
        //    NodeDebugger debugger = null;
        //    var re = new Regex("Debugger listening on ws://([^:]+):([\\d]+)/([0-9a-z\\-]+)");
        //    var exited = false;
        //    while (!exited && !cancellationToken.IsCancellationRequested)
        //    {
        //        var match = re.Match(stdErr.ToString());
        //        if (match.Success)
        //        {
        //            debugger = new NodeDebugger(
        //                SerializerFactory,
        //                match.Groups[1].Value,
        //                int.Parse(match.Groups[2].Value),
        //                match.Groups[3].Value,
        //                cancellationToken);
        //            break;
        //        }
        //        exited = process.WaitForExit(10);
        //    }

        //    if (debugger != null)
        //    {
        //        debugger.Connect();
        //    }
        //    return debugger;
        //}
    }
}
