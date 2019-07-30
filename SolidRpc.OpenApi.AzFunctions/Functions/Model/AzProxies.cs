﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// Represents a proxies json file
    /// </summary>
    public class AzProxies
    {
        /// <summary>
        /// The schema
        /// </summary>
        [DataMember(Name = "$schema", EmitDefaultValue = false)]
        public string Schema { get; set; }

        /// <summary>
        /// The schema
        /// </summary>
        [DataMember(Name = "proxies", EmitDefaultValue = false)]
        public IDictionary<string, AzProxy> Proxies { get; set; }

    }
}
