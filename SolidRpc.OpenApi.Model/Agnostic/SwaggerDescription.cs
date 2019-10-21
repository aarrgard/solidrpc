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
        /// <summary>
        /// Creates a new description
        /// </summary>
        /// <param name="description"></param>
        /// <param name="externalDescription"></param>
        /// <param name="externalUri"></param>
        /// <returns></returns>
        public static SwaggerDescription Create(string description, string externalDescription, string externalUri)
        {
            if (description == null) return null;
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

        /// <summary>
        /// the external uri
        /// </summary>
        public string ExternalUri { get; set; }

        /// <summary>
        /// The external description
        /// </summary>
        public string ExternalDescription { get; set; }
    }
}
