﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a CSharp method
    /// </summary>
    public class CSharpProperty
    {
        /// <summary>
        /// The return type.
        /// </summary>
        public CSharpObject PropertyType { get; set; }

        /// <summary>
        /// Information about the "DataMember" attribute.
        /// </summary>
        public CSharpDataMember DataMember { get; set; }
        
        /// <summary>
        /// The property name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The description of the property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is this property required
        /// </summary>
        public bool Required { get; set; }
    }
}
