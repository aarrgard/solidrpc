using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Parameter : Member, IParameter
    {
        public Parameter(IMethod method, string name) : base(method)
        {
            Name = name;
        }

        public string Description { get; set; }

        public override string Name { get; }

        public IClass ParameterType { get; set; }

        public string DefaultValue { get; set; }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"{SimplifyName(ParameterType.FullName)} {Name}");
            if(!string.IsNullOrEmpty(DefaultValue))
            {
                codeWriter.Emit($" = {SimplifyName(DefaultValue)}");
            }
        }

        public override void GetNamespaces(ICollection<string> namespaces)
        {
            namespaces.Add(ParameterType.Namespace);
            base.GetNamespaces(namespaces);
        }
    }
}