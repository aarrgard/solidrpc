using System;
using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpMethodParameter : CSharpMember, ICSharpMethodParameter
    {
        public CSharpMethodParameter(ICSharpMember parent, string name, ICSharpType parameterType, bool optional) : base(parent, name)
        {
            ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            Optional = optional;
        }

        public ICSharpType ParameterType { get; }

        public bool Optional { get; }
    }
}