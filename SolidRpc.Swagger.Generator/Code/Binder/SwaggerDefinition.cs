using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Code.Binder
{
    /// <summary>
    /// Represents a swagger object.
    /// </summary>
    public class SwaggerDefinition
    {
        public static SwaggerDefinition Void = new SwaggerDefinition() { Name = "void" };

        private string _name;

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name {
            get
            {
                return _name;
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                _name = value;
            }
        }

        public bool IsArray { get; set; }
    }
}
