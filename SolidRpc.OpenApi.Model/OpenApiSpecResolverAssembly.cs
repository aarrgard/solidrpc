using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

[assembly: SolidRpcAbstractionProvider(typeof(IOpenApiSpecResolver), typeof(OpenApiSpecResolverAssembly))]
namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// Resolves specifications that are embedded in an assembly.
    /// </summary>
    public class OpenApiSpecResolverAssembly : IOpenApiSpecResolver
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public OpenApiSpecResolverAssembly(IOpenApiParser openApiParser)
        {
            OpenApiParser = openApiParser ?? throw new System.ArgumentNullException(nameof(openApiParser));
            OpenApiSpecs = new ConcurrentDictionary<string, IOpenApiSpec>();
            Assemblies = new HashSet<Assembly>();
        }

        private ConcurrentDictionary<string, IOpenApiSpec> OpenApiSpecs { get; }
        private ICollection<Assembly> Assemblies { get; }
        private IOpenApiParser OpenApiParser { get; }

        private (Assembly, string) GetManifestResource(string address)
        {
            //
            // hacky solution to to ignore relative paths - find a better solution.
            // the resolver should be able to find resources based on relative paths.
            //
            if(address.StartsWith("../"))
            {
                address = address.Substring(3);
            } 
            foreach(var a in Assemblies)
            {
                var resName = a.GetManifestResourceNames()
                    .Where(o => o.EndsWith($"{address}"))
                    .FirstOrDefault();
                if(!string.IsNullOrEmpty(resName))
                {
                    return (a, resName);
                } 
            }
            return (null, null);
        }

        /// <summary>
        /// Adds supplied assembly to set of assemblies to look for api spec:S
        /// </summary>
        /// <param name="assembly"></param>
        public void AddAssemblyResources(Assembly assembly)
        {
            Assemblies.Add(assembly);
        }

        /// <summary>
        /// Resolves the api spec with supplied name
        /// </summary>
        /// <param name="address"></param>
        /// <param name="openApiSpec"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public bool TryResolveApiSpec(string address, out IOpenApiSpec openApiSpec, string basePath = null)
        {
            openApiSpec = OpenApiSpecs.GetOrAdd(address, _ => {
                var (a, resName) = GetManifestResource(address);
                if (resName == null) return null;
                using (var s = a.GetManifestResourceStream(resName))
                using (var sr = new StreamReader(s))
                {
                    var spec = OpenApiParser.ParseSpec(this, address, sr.ReadToEnd());
                    spec.SetOpenApiSpecResolver(this, address);
                    return spec;
                }
            });
            return openApiSpec != null;
        }
    }
}
