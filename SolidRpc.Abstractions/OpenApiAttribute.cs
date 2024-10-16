﻿using System;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// Configures the openapi
    /// </summary>
    public class OpenApiAttribute : Attribute
    {
        /// <summary>
        /// The name of the method or parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Where should the parameter be put.
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// The verbs to use.
        /// </summary>
        public string[] Verbs { get; set; }

        /// <summary>
        /// The path to bind.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The separator to use in lists
        /// </summary>
        public string CollectionFormat { get; set; }
    }
}
