﻿namespace SolidRpc.Swagger.Generator.Code.Binder
{
    public class CSharpMethodParameter
    {
        /// <summary>
        /// The parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameter type.
        /// </summary>
        public CSharpObject ParameterType { get; set; }
    }
}