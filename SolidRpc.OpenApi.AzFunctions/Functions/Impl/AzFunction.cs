using Newtonsoft.Json;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using System;
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
                return SerializeFunctionJson(Function);
            }
            set
            {
                Function = DeserializeFunction(value);
            }
        }

        private Function DeserializeFunction(string value)
        {
            return JsonConvert.DeserializeObject<Function>(value);
        }

        private string SerializeFunctionJson(Function function)
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
                serializer.Serialize(writer, function);
            }
            return sw.ToString();
        }

        private string ReadFunctionJson()
        {
            var fi = new FileInfo(Path.Combine(Directory.FullName, "function.json"));
            if(!fi.Exists)
            {
                return "";
            }
            using (var tr = fi.OpenText())
            {
                return tr.ReadToEnd();
            }
        }

        private void WriteFunctionJson(string suffix)
        {
            var fi = new FileInfo(Path.Combine(Directory.FullName, "function.json"));
            using (var tw = fi.CreateText())
            {
                tw.Write(SerializeFunctionJson(Function));
                tw.Write(suffix);
            }
            var projectDir = fi.Directory?.Parent?.Parent?.Parent?.Parent;
            if(projectDir == null || !projectDir.Exists)
            {
                return;
            }
            var projectHostFile = new FileInfo(Path.Combine(projectDir.FullName, "host.json"));
            if(projectHostFile.Exists)
            {

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
        public bool Save(bool forceWrite = false)
        {
            if(!Directory.Exists)
            {
                Directory.Create();
            }

            if(forceWrite)
            {
                WriteFunctionJson($"//{DateTime.Now.ToString("yyyy-MM-dd:HH:mm:ss.fffff")}");
                return true;
            }
            else if(false==string.Equals(FunctionJson, SerializeFunctionJson(DeserializeFunction(ReadFunctionJson()))))
            {
                WriteFunctionJson("");
                return true;
            }
            return false;
        }
    }
}
