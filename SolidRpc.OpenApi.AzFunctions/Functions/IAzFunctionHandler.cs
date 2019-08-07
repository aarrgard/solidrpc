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
        IEnumerable<IAzFunction> Functions { get; }

        /// <summary>
        /// Returns the http scheme.
        /// </summary>
        string HttpScheme { get; }

        /// <summary>
        /// Returns the http route prefix.
        /// </summary>
        string HttpRoutePrefix { get; }

        /// <summary>
        /// Creates a new timer function
        /// </summary>
        /// <returns></returns>
        IAzTimerFunction CreateTimerFunction(string name);

        /// <summary>
        /// Creates a http function
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IAzHttpFunction CreateHttpFunction(string name);

        /// <summary>
        /// Triggers a restart by writing some additional data to 
        /// the end of a config file
        /// </summary>
        void TriggerRestart();

        /// <summary>
        /// Puts all the functions in the proxies file.
        /// </summary>
        void SyncProxiesFile();

        /// <summary>
        /// Returns the http trigger handler.
        /// </summary>
        Type HttpTriggerHandler { get; }

        /// <summary>
        /// Returns the timer trigger handler.
        /// </summary>
        Type TimerTriggerHandler { get; }

        /// <summary>
        /// The location of the functions app
        /// </summary>
        DirectoryInfo BaseDir { get; }

        /// <summary>
        /// The development directory.
        /// </summary>
        DirectoryInfo DevDir { get; }

        /// <summary>
        /// Creates a vaid route from supplied route. V1 & V2 handles initial
        /// slashes differently.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        string CreateRoute(string route);
    }
}
