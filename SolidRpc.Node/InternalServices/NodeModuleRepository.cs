using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Node.InternalServices;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INodeModulesRepository), typeof(NodeModuleRepository), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.Node.InternalServices
{
    public class NodeModuleRepository : INodeModulesRepository
    {
        private class PackageInfo
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "version")]
            public string Version { get; set; }
        }


        public NodeModuleRepository(IEnumerable<INodeModuleResolver> resolvers, ISerializerFactory serializerFactory)
        {
            Resolvers = resolvers;
            SerializerFactory = serializerFactory;
            NodeModules = new ConcurrentDictionary<Guid, Task<string>>();
        }

        private IEnumerable<INodeModuleResolver> Resolvers { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ConcurrentDictionary<Guid, Task<string>> NodeModules { get; }

        public Task<string> GetNodeModulePathAsync(Guid moduleId, CancellationToken cancellationToken = default)
        {
            var module = NodeModules.GetOrAdd(moduleId, _ => CreateNodeModule(_, cancellationToken));
            if(module.IsFaulted)
            {
                NodeModules.TryRemove(moduleId, out module);
                module = NodeModules.GetOrAdd(moduleId, _ => CreateNodeModule(_, cancellationToken));
            }
            return module;
        }

        private async Task<string> CreateNodeModule(Guid moduleId, CancellationToken cancellationToken)
        {
            var path = new DirectoryInfo(Path.Combine(Path.GetTempPath(), moduleId.ToString()));
            path.Create();

            foreach (var resolver in Resolvers)
            {
                if(resolver.ModuleId == moduleId)
                {
                    await resolver.ExplodeNodeModulesAsync(path, cancellationToken);
                    return Path.Combine(path.FullName, "node_modules");
                }
            }

            return Path.Combine(path.FullName, "node_modules");
        }

        public async Task<IEnumerable<NodeModules>> GetNodeModulesAsync(CancellationToken cancellationToken = default)
        {
            var modules = Resolvers.Select(o => new NodeModules() { Id = o.ModuleId }).ToArray();
            var moduleTasks = modules.Select(o => GetNodeModulePathAsync(o.Id, cancellationToken));
            var paths = await Task.WhenAll(moduleTasks);
            var packageTasks = paths.Select(o => GetPackagesAsync(o, cancellationToken));
            var packages = await Task.WhenAll(packageTasks);
            for (int i = 0; i < paths.Length; i++)
            {
                modules[i].Packages = packages[i];
            }
            return modules;
        }
        private async Task<IEnumerable<NodePackage>> GetPackagesAsync(string path, CancellationToken cancellationToken)
        {
            var packages = new List<NodePackage>();
            var dir = new DirectoryInfo(path);
            foreach(var moduleDir in dir.GetDirectories())
            {
                var jsonFile = new FileInfo(Path.Combine(moduleDir.FullName, "package.json"));
                if (!jsonFile.Exists) continue;
                using (var fs = jsonFile.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    SerializerFactory.DeserializeFromStream(fs, out PackageInfo packageInfo);
                    packages.Add(new NodePackage() { Name = packageInfo.Name, Version = packageInfo.Version });
                }
            }
            return packages;
        }
    }
}
