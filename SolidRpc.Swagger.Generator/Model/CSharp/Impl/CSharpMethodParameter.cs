using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpMethodParameter : CSharpMember, ICSharpMethodParameter
    {
        public CSharpMethodParameter(ICSharpMember parent, string name, ICSharpType parameterType) : base(parent, name)
        {
            ParameterType = parameterType;
        }

        public ICSharpType ParameterType { get; set; }
    }
}