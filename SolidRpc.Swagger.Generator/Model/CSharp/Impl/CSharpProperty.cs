﻿using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpProperty : CSharpMember, ICSharpProperty
    {
        public CSharpProperty(ICSharpMember parent, string name, ICSharpType propertyType) : base(parent, name)
        {
            PropertyType = propertyType;
        }
        public ICSharpType PropertyType { get; }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"/// <summary>{codeWriter.NewLine}");
            codeWriter.Emit($"/// {Comment?.Summary}{codeWriter.NewLine}");
            codeWriter.Emit($"/// </summary>{codeWriter.NewLine}");
            codeWriter.Emit($"public {SimplifyName(PropertyType.FullName)} {Name} {{ get; set; }}{codeWriter.NewLine}");
        }

        public override void GetNamespaces(ICollection<string> namespaces)
        {
            namespaces.Add(PropertyType.Namespace.FullName);
            base.GetNamespaces(namespaces);
        }
    }
}