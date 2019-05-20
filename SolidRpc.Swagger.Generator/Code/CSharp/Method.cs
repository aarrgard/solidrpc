using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Method : IMethod
    {
        public Method(IMember member, string methodName)
        {
            Member = member;
            Name = methodName;
            Parameters = new List<IParameter>();
        }

        public IMember Member { get; }

        public string Name { get; }

        public IEnumerable<IMember> Members => new IMember[0];

        public IClass ReturnType { get; set; }

        public IList<IParameter> Parameters { get; }

        IEnumerable<IParameter> IMethod.Parameters => Parameters;

        public IParameter AddParameter(string parameterName)
        {
            var p = new Parameter(this, parameterName);
            Parameters.Add(p);
            return p;
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"{ReturnType.Name} {Name}(");
            codeWriter.Emit($");{codeWriter.NewLine}");
        }
    }
}