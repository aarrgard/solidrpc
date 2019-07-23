using Newtonsoft.Json;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Impl
{
    /// <summary>
    /// Base class for the functions
    /// </summary>
    public abstract class AzFunction : IAzFunction
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="function"></param>
        /// <param name="runCsx"></param>
        public AzFunction(DirectoryInfo directory, Function function)
        {
            Directory = directory;
            Function = function;
        }

        /// <summary>
        /// The directory where the function resides
        /// </summary>
        public DirectoryInfo Directory { get; }

        /// <summary>
        /// The trigger type
        /// </summary>
        public abstract string TriggerType { get; }

        /// <summary>
        /// The name of the function
        /// </summary>
        public string Name => Directory.Name;

        /// <summary>
        /// The function
        /// </summary>
        public Function Function { get; set; }

        /// <summary>
        /// The trigger binding
        /// </summary>
        public Binding TriggerBinding => Function.Bindings.Single(o => o.Type == TriggerType);

        /// <summary>
        /// The function
        /// </summary>
        public string FunctionJson {
            get
            {
                var sw = new StringWriter();
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    var settings = new JsonSerializerSettings()
                    {
                        ContractResolver = NewtonsoftContractResolver.Instance,
                        Formatting = Formatting.Indented
                    };
                    var serializer = JsonSerializer.Create(settings);
                    serializer.Serialize(writer, Function);
                }
                return sw.ToString();
            }
            set
            {
                Function = JsonConvert.DeserializeObject<Function>(value);
            }
        }

        /// <summary>
        /// Deletes the function
        /// </summary>
        public void Delete()
        {
            Directory.Delete(true);
        }

        /// <summary>
        /// Saves the run.csx and function.json files
        /// </summary>
        public void Save(bool forceWrite = false)
        {
            if(!Directory.Exists)
            {
                Directory.Create();
            }
            WriteFile(new FileInfo(Path.Combine(Directory.FullName, "function.json")), FunctionJson, forceWrite);
        }

        private void WriteFile(FileInfo fileInfo, string newContent, bool forceWrite)
        {
            if(!forceWrite && fileInfo.Exists)
            {
                using (var tr = fileInfo.OpenText())
                {
                    var existingContent = tr.ReadToEnd();
                    if(existingContent.Equals(newContent))
                    {
                        return;
                    }
                }
            }
            using (var tw = fileInfo.CreateText())
            {
                tw.Write(newContent);
            }
        }
    }
}
