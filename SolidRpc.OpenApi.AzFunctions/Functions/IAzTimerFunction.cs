using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.AzFunctions.Functions
{
    /// <summary>
    /// Represents a timer function
    /// </summary>
    public interface IAzTimerFunction : IAzFunction
    {
        /// <summary>
        /// Handles the shhedule
        /// </summary>
        string Schedule { get; set; }

        /// <summary>
        /// Specifies if the trigger should run on startup.
        /// </summary>
        bool RunOnStartup { get; set; }

        /// <summary>
        /// The service type to resolve
        /// </summary>
        string ServiceType { get; set; }

        /// <summary>
        /// The method to invoke on the service
        /// </summary>
        string MethodName { get; set; }
    }
}
