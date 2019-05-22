using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Property : Member, IProperty
    {

        public Property(IClass parent, IClass propertyType, string propertyName) : base(parent)
        {
            Name = propertyName;
            PropertyType = propertyType;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name { get; }

        public IClass PropertyType { get; }

        public string Summary { get; set; }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"/// <summary>{codeWriter.NewLine}");
            codeWriter.Emit($"/// {Summary}{codeWriter.NewLine}");
            codeWriter.Emit($"/// </summary>{codeWriter.NewLine}");
            codeWriter.Emit($"public {SimplifyName(PropertyType.FullName)} {Name} {{ get; set; }}{codeWriter.NewLine}");
        }
    }
}