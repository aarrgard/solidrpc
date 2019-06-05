using SolidRpc.OpenApi.Generator.Code.Binder;
using SolidRpc.OpenApi.Generator.Model.CSharp;
using SolidRpc.OpenApi.Generator.Model.CSharp.Impl;
using SolidRpc.OpenApi.Generator.V2;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.OpenApi.Model.V3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Generator
{
    /// <summary>
    /// Code generator to create interfaces and classes from a swagger specification.
    /// </summary>
    public abstract class OpenApiCodeGenerator
    {
        protected OpenApiCodeGenerator(OpenApiCodeSettings codeSettings)
        {
            CodeSettings = codeSettings;
        }

        public OpenApiCodeSettings CodeSettings { get; }

        /// <summary>
        /// Generates code 
        /// </summary>
        /// <param name="codeSettings"></param>
        public static void GenerateCode(OpenApiCodeSettings codeSettings)
        {
            var codeGenerator = (ICSharpRepository)new CSharpRepository();

            var model = OpenApiParser.ParseSwaggerSpec(codeSettings.SwaggerSpec);
            if (model is SwaggerObject v2)
            {
                new OpenApiCodeGeneratorV2(v2, codeSettings).GenerateCode(codeGenerator);
            }
            else if (model is OpenAPIObject v3)
            {
                new OpenApiCodeGeneratorV3(v3, codeSettings).GenerateCode(codeGenerator);
            }
            else
            {
                throw new Exception("Cannot parse swagger json.");
            }

            var codeWriter = new CodeWriterFile(codeSettings.OutputPath, codeSettings.ProjectNamespace);
            codeGenerator.WriteCode(codeWriter);
            codeWriter.Close();
        }
        protected abstract void GenerateCode(ICSharpRepository codeGenerator);

        protected ICSharpClass GetClass(ICSharpRepository csharpRepository, CSharpObject cSharpObject)
        {
            var cls = csharpRepository.GetClass(cSharpObject.Name);
            if(!cls.Initialized)
            {
                cls.Initialized = true;
                if (cSharpObject.AdditionalProperties != null)
                {
                    var dictType = csharpRepository.GetClass(cSharpObject.AdditionalProperties.Name);
                    var extType = csharpRepository.GetClass($"System.Collections.Generic.Dictionary<string,{dictType.FullName}>");
                    cls.AddExtends(extType);
                }
                // add missing properties
                foreach (var prop in cSharpObject.Properties)
                {
                    var propType = GetClass(csharpRepository, prop.PropertyType);
                    var csProp = new Model.CSharp.Impl.CSharpProperty(cls, prop.PropertyName, propType);
                    csProp.ParseComment($"<summary>{prop.Description}</summary>");
                    cls.AddMember(csProp);
                }
                if (cSharpObject.ArrayElement != null)
                {
                    GetClass(csharpRepository, cSharpObject.ArrayElement);
                }
                AddUsings(cls);
            }
            return cls;
        }

        protected void AddUsings(ICSharpType member)
        {
            var namespaces = new HashSet<string>();
            member.GetNamespaces(namespaces);
            if (namespaces.Any(o => o.Contains("<")))
            {
                throw new Exception();
            }
            namespaces.Where(o => !string.IsNullOrEmpty(o)).ToList().ForEach(ns =>
            {
                AddUsings(member, ns);
            });
        }

        private void AddUsings(ICSharpType member, string ns)
        {
            var usings = member.Members.OfType<ICSharpUsing>().Select(o => o.Name);
            if (!usings.Contains(ns)) member.AddMember(new CSharpUsing(member, ns));
        }
    }
}
