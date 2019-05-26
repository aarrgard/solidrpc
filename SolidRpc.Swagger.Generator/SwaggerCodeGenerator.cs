using SolidRpc.Swagger.Generator.Code;
using SolidRpc.Swagger.Generator.Code.Binder;
using SolidRpc.Swagger.Generator.Code.CSharp;
using SolidRpc.Swagger.Generator.V2;
using SolidRpc.Swagger.Model;
using SolidRpc.Swagger.Model.V2;
using SolidRpc.Swagger.Model.V3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator
{
    /// <summary>
    /// Code generator to create interfaces and classes from a swagger specification.
    /// </summary>
    public abstract class SwaggerCodeGenerator
    {
        protected SwaggerCodeGenerator(SwaggerCodeSettings codeSettings)
        {
            CodeSettings = codeSettings;
        }

        public SwaggerCodeSettings CodeSettings { get; }

        /// <summary>
        /// Generates code 
        /// </summary>
        /// <param name="codeSettings"></param>
        public static void GenerateCode(SwaggerCodeSettings codeSettings)
        {
            var codeGenerator = (ICodeGenerator)new CodeGenerator();

            var model = SwaggerParser.ParseSwaggerSpec(codeSettings.SwaggerSpec);
            if (model is SwaggerObject v2)
            {
                new SwaggerCodeGeneratorV2(v2, codeSettings).GenerateCode(codeGenerator);
            }
            else if (model is OpenAPIObject v3)
            {
                new SwaggerCodeGeneratorV3(v3, codeSettings).GenerateCode(codeGenerator);
            }
            else
            {
                throw new Exception("Cannot parse swagger json.");
            }

            var codeWriter = new CodeWriterFile(codeSettings.OutputPath, codeSettings.ProjectNamespace);
            codeGenerator.WriteCode(codeWriter);
            codeWriter.Close();
        }
        protected abstract void GenerateCode(ICodeGenerator codeGenerator);

        protected IClass GetClass(ICodeGenerator codeGenerator, CSharpObject cSharpObject)
        {
            var ns = codeGenerator.GetNamespace(cSharpObject.Name.Namespace);
            var cls = ns.GetClass(cSharpObject.Name.Name);
            // add missing properties
            foreach (var prop in cSharpObject.Properties)
            {
                if(cls.Properties.Any(o => o.Name == prop.PropertyName))
                {
                    continue;
                }
                var propType = GetClass(codeGenerator, prop.PropertyType);
                cls.AddProperty(prop.PropertyName, propType).Summary = prop.Description;
            }
            if(cSharpObject.ArrayElement != null)
            {
                return GetClass(codeGenerator, cSharpObject.ArrayElement);
            }
            AddUsings(cls);
            return cls;
        }

        protected void AddUsings(IMember member)
        {
            var namespaces = new HashSet<string>();
            member.GetNamespaces(namespaces);
            namespaces.Where(o => !string.IsNullOrEmpty(o)).ToList().ForEach(ns =>
            {
                AddUsings(member, ns);
            });
        }

        private void AddUsings(IMember member, string ns)
        {
            var usings = member.Members.OfType<Using>().Select(o => o.Name);
            if (!usings.Contains(ns)) member.Members.Add(new Using(member, ns));
        }
    }
}
