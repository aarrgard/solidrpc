using System;
using System.Collections.Generic;
using System.IO;

namespace SolidRpc.OpenApi.AzFunctions.Functions
{
    /// <summary>
    /// The function handler handles the functions in the application
    /// </summary>
    public interface IAzFunctionHandler
    {
        /// <summary>
        /// Returns the functions
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAzFunction> GetFunctions();

        /// <summary>
        /// Returns the http scheme.
        /// </summary>
        string HttpScheme { get; }

        /// <summary>
        /// Returns the http route prefix.
        /// </summary>
        string HttpRouteBackendPrefix { get; }

        /// <summary>
        /// The frontend prefix
        /// </summary>
        string HttpRouteFrontendPrefix { get; }

        /// <summary>
        /// Returns the prefix mappings
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetPrefixMappings();

        /// <summary>
        /// Creates a new timer function
        /// </summary>
        /// <returns></returns>
        T CreateFunction<T>(string name) where T : class, IAzFunction;

        /// <summary>
        /// Creates a new timer function
        /// </summary>
        /// <returns></returns>
        T GetOrCreateFunction<T>(string name) where T : class, IAzFunction;

        /// <summary>
        /// Triggers a restart by writing some additional data to 
        /// the end of a config file
        /// </summary>
        void TriggerRestart();

        /// <summary>
        /// Puts all the functions in the proxies file.
        /// </summary>
        void SyncProxiesFile(
            IDictionary<string, string> staticRoutes, 
            IDictionary<string, string> redirects);

        /// <summary>
        /// Returns the http trigger handler.
        /// </summary>
        Type HttpTriggerHandler { get; }

        /// <summary>
        /// Returns the timer trigger handler.
        /// </summary>
        Type TimerTriggerHandler { get; }


        /// <summary>
        /// Returns the queue trigger handler.
        /// </summary>
        Type QueueTriggerHandler { get; }

        /// <summary>
        /// The location of the functions app
        /// </summary>
        IEnumerable<DirectoryInfo> BaseDirs { get; }

        /// <summary>
        /// Creates a vaid route from supplied route. V1 &amp; V2 handles initial
        /// slashes differently.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        string CreateRoute(string route);
    }
}
