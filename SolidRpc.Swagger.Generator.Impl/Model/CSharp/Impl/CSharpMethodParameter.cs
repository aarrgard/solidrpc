﻿using System;
using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpMethodParameter : CSharpMember, ICSharpMethodParameter
    {
        public CSharpMethodParameter(ICSharpMember parent, string name, ICSharpType parameterType, bool optional, string defaultValue = null) : base(parent, name)
        {
            ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            Optional = optional;
            DefaultValue = defaultValue;
        }

        public ICSharpType ParameterType { get; }

        public bool Optional { get; }

        public string DefaultValue { get; }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"{SimplifyName(ParameterType.FullName)} {Name}");
            if (!string.IsNullOrEmpty(DefaultValue))
            {
                codeWriter.Emit($" = {SimplifyName(DefaultValue)}");
            }
        }

        public override void GetNamespaces(ICollection<string> namespaces)
        {
            namespaces.Add(ParameterType.Namespace.FullName);
            base.GetNamespaces(namespaces);
        }
    }
}