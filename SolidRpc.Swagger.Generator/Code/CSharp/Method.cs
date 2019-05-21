using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Method : IMethod
    {
        public Method(IMember member, string methodName)
        {
            Member = member;
            Name = methodName;
            Members = new List<IMember>();
        }

        public IMember Member { get; }

        public string Name { get; }

        public IList<IMember> Members { get; }

        IEnumerable<IMember> IMember.Members => Members;

        public IClass ReturnType { get; set; }

        public IEnumerable<IParameter> Parameters => Members.OfType<IParameter>();

        public IParameter AddParameter(string parameterName)
        {
            var p = new Parameter(this, parameterName);
            Members.Add(p);
            return p;
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"{ReturnType.FullName} {Name}(");
            var parameters = Members.OfType<IParameter>().ToList();
            for(int i = 0; i < parameters.Count; i ++)
            {
                codeWriter.Emit(codeWriter.NewLine);
                codeWriter.Indent();
                parameters[i].WriteCode(codeWriter);
                if(i < parameters.Count - 1)
                {
                    codeWriter.Emit(",");
                }
                codeWriter.Unindent();
            }
            codeWriter.Emit($");{codeWriter.NewLine}");
        }
    }
}