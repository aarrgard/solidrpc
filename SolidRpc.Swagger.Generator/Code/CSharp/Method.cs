using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Method : Member, IMethod
    {
        public Method(IMember member, string methodName) : base(member)
        {
            Name = methodName;
        }

        public string Summary { get; set; }

        public override string Name { get; }

        public IClass ReturnType { get; set; }

        public IEnumerable<IParameter> Parameters => Members.OfType<IParameter>();

        public IParameter AddParameter(string parameterName)
        {
            var p = new Parameter(this, parameterName);
            Members.Add(p);
            return p;
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            var parameters = Members.OfType<IParameter>().ToList();
            codeWriter.Emit($"/// <summary>{codeWriter.NewLine}");
            codeWriter.Emit($"/// {Summary}{codeWriter.NewLine}");
            codeWriter.Emit($"/// </summary>{codeWriter.NewLine}");
            parameters.ForEach(p =>
            {
                codeWriter.Emit($"/// <param name=\"{p.Name}\">{p.Description}</param>{codeWriter.NewLine}");
            });
            codeWriter.Emit($"{SimplifyName(ReturnType.FullName)} {Name}(");
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