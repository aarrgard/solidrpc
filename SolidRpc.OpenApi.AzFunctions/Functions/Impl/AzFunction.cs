﻿using Newtonsoft.Json;
using SolidRpc.OpenApi.AzFunctions.Functions.Model;
using System;
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
        /// <param name="functionHandler"></param>
        /// <param name="subDir"></param>
        /// <param name="function"></param>
        public AzFunction(IAzFunctionHandler functionHandler, string subDir, Function function)
        {
            FunctionHandler = functionHandler ?? throw new ArgumentNullException(nameof(functionHandler));
            SubDir = subDir ?? throw new ArgumentNullException(nameof(subDir));
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Name = SubDir.Split('/').SelectMany(o => o.Split('\\')).Last();
        }

        /// <summary>
        /// The trigger type
        /// </summary>
        public abstract string TriggerType { get; }

        /// <summary>
        /// The function handler
        /// </summary>
        public IAzFunctionHandler FunctionHandler { get; }

        /// <summary>
        /// Returns all the function dirs
        /// </summary>
        public IEnumerable<DirectoryInfo> FunctionDirs => FunctionHandler
            .BaseDirs.Select(o => new DirectoryInfo(Path.Combine(o.FullName, SubDir)));

        /// <summary>
        /// The function dir
        /// </summary>
        public string SubDir { get; }

        /// <summary>
        /// The name of the function
        /// </summary>
        public string Name { get; }

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

        /// <summary>
        /// Returns the generated by tag.
        /// </summary>
        public string GeneratedBy => Function.GeneratedBy;

        private Function DeserializeFunction(string value)
        {
            var sr = new StringReader(value);
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = NewtonsoftContractResolver.Instance,
                    Formatting = Formatting.Indented
                };
                var serializer = JsonSerializer.Create(settings);
                return serializer.Deserialize<Function>(reader);
            }
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
            return ReadFunctionJson(new FileInfo(Path.Combine(FunctionHandler.BaseDirs.First().FullName, Name, "function.json")));
        }

        private string ReadFunctionJson(FileInfo fi)
        {
            if(!fi.Exists)
            {
                return "";
            }
            using (var tr = fi.OpenText())
            {
                return tr.ReadToEnd();
            }
        }

        private void WriteFunctionJson(FileInfo fi)
        {
            using (var tw = fi.CreateText())
            {
                tw.Write(SerializeFunctionJson(Function));
            }
        }

        /// <summary>
        /// Deletes the function
        /// </summary>
        public void Delete()
        {
            FunctionDirs.ToList().ForEach(o =>
            {
                if (o.Exists)
                {
                    o.Delete(true);
                }
            });
        }

        /// <summary>
        /// Saves the run.csx and function.json files
        /// </summary>
        public bool Save()
        {
            return FunctionDirs.Select(o =>
            {
                return Save(new FileInfo(Path.Combine(o.FullName, "function.json")));
            }).ToList().First(); // Do not remove "ToList()"!
        }
        /// <summary>
        /// Saves the run.csx and function.json files
        /// </summary>
        public bool Save(FileInfo fi)
        {
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            var newFunctionJson = FunctionJson;
            var oldFunctionJson = SerializeFunctionJson(DeserializeFunction(ReadFunctionJson(fi)));
            if (false==string.Equals(newFunctionJson, oldFunctionJson))
            {
                WriteFunctionJson(fi);
                return true;
            }
            return false;
        }
    }
}
