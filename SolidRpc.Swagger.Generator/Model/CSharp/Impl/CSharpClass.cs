﻿using System;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpClass : CSharpType, ICSharpClass
    {
        public CSharpClass(ICSharpNamespace ns, string name, Type runtimeType) : base(ns, name, runtimeType)
        {
        }
    }
}