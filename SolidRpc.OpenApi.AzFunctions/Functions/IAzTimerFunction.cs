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
        /// The timer id
        /// </summary>
        string TimerId { get; set; }
    }
}
