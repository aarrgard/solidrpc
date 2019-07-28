using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a charp description
    /// </summary>
    public class SwaggerDescription
    {
        public static SwaggerDescription Create(string description, string externalDescription, string externalUri)
        {
            return new SwaggerDescription()
            {
                Description = description,
                ExternalDescription = externalDescription,
                ExternalUri = externalUri
            };
        }
        /// <summary>
        /// The summary
        /// </summary>
        public string Description { get; set; }


        public string ExternalUri { get; set; }

        public string ExternalDescription { get; set; }
    }
}
