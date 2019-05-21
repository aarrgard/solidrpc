using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Parameter : IParameter
    {
        public Parameter(IMethod method, string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IMember> Members => new IMember[0];

        public IClass ParameterType { get; set; }

        public void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"{ParameterType.FullName} {Name}");
        }
    }
}