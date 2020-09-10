using Microsoft.Extensions.Logging;
using SolidRpc.NpmGenerator.Debugger;
using SolidRpc.NpmGenerator.Types;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.NpmGenerator.Services
{
    /// <summary>
    /// Implements the node service. Executables are fetched from https://nodejs.org/es/blog/release/v8.17.0/.
    /// </summary>
    public class NodeService : INodeService
    {
        private class ExecutionArg
        {
            public bool UseDebugger { get; set; }
            public string WorkingDirectory { get; set; }
            public CancellationToken CancellationToken { get; set; }
            public string Script { get; set; }
            public string Arguments { get; set; }
        }

        /// <summary>
        /// Constructs the instance
        /// </summary>
        /// <param name="logger"></param>
        public NodeService(ILogger<NodeService> logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; }

        public async Task<NodeExecution> ExecuteJSAsync(string js, CancellationToken cancellationToken = default(CancellationToken))
        {
            //
            // create temp dir
            //
            var uid = Guid.NewGuid().ToString();
            var workingDir = Path.Combine(Path.GetTempPath(), uid);
            Directory.CreateDirectory(workingDir);

            try
            {
                //
                // Run script
                //
                return await Task.Factory.StartNew<NodeExecution>(RunJs, new ExecutionArg()
                {
                    UseDebugger = false,
                    Script = js,
                    CancellationToken = cancellationToken,
                    WorkingDirectory = workingDir,
                }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            finally
            {
                while(Directory.Exists(workingDir))
                {
                    try
                    {
                        Directory.Delete(workingDir, true);
                    }
                    catch
                    {
                        await Task.Delay(100);
                    }
                }
            }
        }

        private NodeExecution RunJs(object oArg)
        {
            var eArg = (ExecutionArg)oArg;
            try
            {
                //
                // determine executable
                //
                var arch = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                var dllLocation = new FileInfo(GetType().Assembly.Location);
                string executableName;
                switch(arch)
                {
                    case "x86":
                        executableName = "node.exe";
                        break;
                    default:
                        throw new Exception($"Cannot handle architecture {arch}");
                }
                var exeFileName = Path.Combine(dllLocation.DirectoryName, "Arch", arch, executableName);

                if(!File.Exists(exeFileName))
                {
                    throw new Exception($"Cannot find the executable {exeFileName}");
                }

                var nodeArgs = eArg.Arguments;
                //
                // write script file if not debug mode
                //
                if(eArg.UseDebugger)
                {
                    nodeArgs = $"--inspect-brk {nodeArgs}";
                }
                else if(!string.IsNullOrEmpty(eArg.Script))
                {
                    //
                    // save script to file
                    //
                    var scriptFileName = Path.Combine(eArg.WorkingDirectory, $"script.js");
                    using (var sw = File.Open(scriptFileName, FileMode.OpenOrCreate, FileAccess.Write))
                    using (var tw = new StreamWriter(sw))
                    {
                        tw.Write(eArg.Script);
                    }
                    nodeArgs = $"{nodeArgs} {scriptFileName}";
                }

                //
                //
                //
                var nodePath = Environment.GetEnvironmentVariable("NODE_PATH");
                var stdOut = new StringBuilder();
                var stdErr = new StringBuilder();
                int? exitCode = null;
                var process = new Process();
                try
                {
                    process.StartInfo.FileName = exeFileName;
                    process.StartInfo.Arguments = nodeArgs.Trim();
                    if(!string.IsNullOrEmpty(eArg.WorkingDirectory))
                    {
                        var di = new DirectoryInfo(eArg.WorkingDirectory);
                        process.StartInfo.WorkingDirectory = eArg.WorkingDirectory;
                        process.StartInfo.EnvironmentVariables["NODE_PATH"] = Path.Combine(di.Parent.FullName, "node_modules");
                    }
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    process.OutputDataReceived += (sender, data) =>
                    {
                        if(!string.IsNullOrEmpty(data.Data))
                        {
                            stdOut.Append(data.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, data) =>
                    {
                        if (!string.IsNullOrEmpty(data.Data))
                        {
                            stdErr.Append(data.Data);
                        }
                    };
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // connect debugger
                    var cancellationToken = eArg.CancellationToken;
                    var debugger = WaitForDebugger(process, stdErr, cancellationToken);

                    // wait for script to complete
                    while(exitCode == null && !cancellationToken.IsCancellationRequested)
                    {
                        if (process.WaitForExit(100))
                        {
                            exitCode = process.ExitCode;
                        }
                    }
                }
                finally
                {
                    if(exitCode == null)
                    {
                        process.Kill();
                    }
                    process.Close();
                    process.Dispose();
                }

                return new NodeExecution()
                {
                    ExitCode = exitCode ?? -1,
                    Out = stdOut.ToString(),
                    Err = stdErr.ToString()
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

        private NodeDebugger WaitForDebugger(Process process, StringBuilder stdErr, CancellationToken cancellationToken)
        {
            //
            // grab debug port from std.err
            //
            NodeDebugger debugger = null;
            var re = new Regex("Debugger listening on ws://([^:]+):([\\d]+)/([0-9a-z\\-]+)");
            var exited = false;
            while (!exited && !cancellationToken.IsCancellationRequested)
            {
                var match = re.Match(stdErr.ToString());
                if (match.Success)
                {
                    debugger = new NodeDebugger(
                        match.Groups[1].Value,
                        int.Parse(match.Groups[2].Value),
                        match.Groups[3].Value,
                        cancellationToken);
                    break;
                }
                exited = process.WaitForExit(10);
            }

            if(debugger != null)
            {
                debugger.Connect();
            }
            return debugger;
        }

        public async Task<string> GetNodeVersionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await Task.Factory.StartNew(RunJs, new ExecutionArg()
            {
                Arguments = $"--version"
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            if(res.ExitCode != 0)
            {
                throw new Exception("Failed to get version!");
            }
            return res.Out;
        }

        public Task DownloadPackageAsync(string package, string version, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task<NodeExecution> ExecuteJSInDebugger(string js, CancellationToken cancellationToken = default(CancellationToken))
        {

            //
            // Run script
            //
            return Task.Factory.StartNew(RunJs, new ExecutionArg()
            {
                UseDebugger = true,
                CancellationToken = cancellationToken,
                WorkingDirectory = Path.GetTempPath(),
                Script = js
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
