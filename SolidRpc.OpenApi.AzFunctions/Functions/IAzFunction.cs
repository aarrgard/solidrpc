﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.AzFunctions.Functions
{
    /// <summary>
    /// Represents an azure function
    /// </summary>
    public interface IAzFunction
    {
        /// <summary>
        /// The name of the function.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// handles the function.json file
        /// </summary>
        string FunctionJson { get; set; }

        /// <summary>
        /// Removes this function.
        /// </summary>
        void Delete();

        /// <summary>
        /// Writes the run.csx and function.json files
        /// </summary>
        bool Save(bool forceWrite = false);
    }
}
