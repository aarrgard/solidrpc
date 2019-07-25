﻿using System.Collections.Generic;

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
    }
}