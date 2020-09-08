using Microsoft.Extensions.Logging;
using SolidRpc.NpmGenerator.Types;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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
            public string Arguments { get; set; }
            public string WorkingDirectory { get; internal set; }
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
                // save script to file
                //
                using (var sw = File.Open(Path.Combine(workingDir, $"{uid}.js"), FileMode.OpenOrCreate, FileAccess.Write))
                using (var tw = new StreamWriter(sw))
                {
                    await tw.WriteAsync(js);
                }

                //
                // Run script
                //
                return await Task.Factory.StartNew<NodeExecution>(RunJs, new ExecutionArg()
                {
                    WorkingDirectory = workingDir,
                    Arguments = $"{uid}.js"
                }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            finally
            {
                Directory.Delete(workingDir, true);
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
                var fileName = Path.Combine(dllLocation.DirectoryName, "Arch", arch, executableName);

                if(!File.Exists(fileName))
                {
                    throw new Exception($"Cannot find the executable {fileName}");
                }

                //
                //
                //
                var nodePath = Environment.GetEnvironmentVariable("NODE_PATH");
                var stdOut = new StringBuilder();
                var stdErr = new StringBuilder();
                using (var process = new Process())
                {
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Arguments = eArg.Arguments;
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

                    process.OutputDataReceived += (sender, data) => stdOut.Append(data.Data);
                    process.ErrorDataReceived += (sender, data) => stdErr.Append(data.Data);
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                
                    process.WaitForExit();
                
                    return new NodeExecution()
                    {
                        ExitCode = process.ExitCode,
                        Out = stdOut.ToString(),
                        Err = stdErr.ToString()
                    };
                }

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

        public async Task<string> GetNodeVersionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await Task.Factory.StartNew<NodeExecution>(RunJs, new ExecutionArg()
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
    }
}
