﻿namespace SolidRpc.Swagger.Generator.Code.Binder
{
    public class CSharpMethodParameter
    {
        /// <summary>
        /// The parameter description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameter type.
        /// </summary>
        public CSharpObject ParameterType { get; set; }

        /// <summary>
        /// Specifies if this parameter is optional
        /// </summary>
        public bool Optional { get; set; }
    }
}