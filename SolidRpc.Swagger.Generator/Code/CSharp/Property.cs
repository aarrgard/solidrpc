using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Property : IProperty
    {

        public Property(IClass propertyType, string propertyName)
        {
            Name = propertyName;
            PropertyType = propertyType;
        }

        public string Name { get; }

        public IClass PropertyType { get; }

        public IEnumerable<IMember> Members => new IMember[0];

        public void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"public {PropertyType.FullName} {Name} {{ get; set; }}{codeWriter.NewLine}");
        }
    }
}