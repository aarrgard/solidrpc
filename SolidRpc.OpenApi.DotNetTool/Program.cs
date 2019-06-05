using Microsoft.Extensions.DependencyInjection;
using SolidRpc.OpenApi.Generator;
using SolidRpc.OpenApi.Generator.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SolidRpc.OpenApi.DotNetTool
{
    class Program
    {
        private static ConcurrentDictionary<string, XmlDocument> ProjectDocuments = new ConcurrentDictionary<string, XmlDocument>();
        private static IServiceProvider s_serviceProvider;

        static void Main(string[] args)
        {
            Console.WriteLine("Running swagger-generator");
            var argList = new List<string>(args);
            var command = argList.Where(o => o == "-code2swagger" || o == "-swagger2code").SingleOrDefault();
            if(command == null)
            {
                Console.Error.WriteLine($"Must supply direction of code: -code2swagger or -swagger2code");
                Environment.Exit(1);
            }
            argList.Remove(command);

            var workingDir = Directory.GetCurrentDirectory();
            var files = argList.Select(o => new FileInfo(Path.Combine(workingDir, o))).ToList();

            switch(command)
            {
                case "-code2swagger":
                    GenerateSwaggerFromCode(files);
                    break;
                case "-swagger2code":
                    GenerateCodeFromSwagger(files);
                    break;
            }
        }

        private static void GenerateSwaggerFromCode(IEnumerable<FileInfo> files)
        {
            if (files.Count() == 0)
            {
                Console.Error.WriteLine($"Must specify file to generate");
                Environment.Exit(1);
            }
            if (files.Count() > 1)
            {
                Console.Error.WriteLine($"Cannot specify more than one file when generating swagger from code.");
                Environment.Exit(1);
            }
            GenerateSwaggerFromCode(files.First());
        }

        private static void GenerateSwaggerFromCode(FileInfo fileInfo)
        {
            var sp = GetServiceProvider();
            // find the project assembly.
            //var assembly = FindAssembly(fileInfo.DirectoryName);
            var settings = new OpenApiSpecSettings()
            {
                Title = GetAssemblyName(),
                Version = GetProjectSetting("SwaggerVersion", "Version") ?? "1.0.0",
                Description = GetProjectSetting("SwaggerDescription", "Description"),
                LicenseName = GetProjectSetting("SwaggerLicenseName", "PackageLicenseUrl"),
                LicenseUrl = GetProjectSetting("SwaggerLicenseUrl", "PackageLicenseUrl"),
                ContactEmail = GetProjectSetting("SwaggerContactEmail"),
                ContactName = GetProjectSetting("SwaggerContactName", "Authors"),
                ContactUrl = GetProjectSetting("SwaggerContactUrl", "PackageProjectUrl"),
                SwaggerFile = fileInfo.FullName,
                CodePath = fileInfo.DirectoryName,
                BasePath = $"/{GetAssemblyName().Replace('.', '/')}",  
                ProjectNamespace = GetProjectNamespace(),
            };
            OpenApiSpecGenerator.GenerateCode(settings);
        }

        private static IServiceProvider GetServiceProvider()
        {
            if(s_serviceProvider == null)
            {
                var sc = new ServiceCollection();
                sc.AddTransient<IOpenApiGenerator, IOpenApiGenerator>();
                s_serviceProvider = sc.BuildServiceProvider();
            }
            return s_serviceProvider;
        }

        private static string GetProjectSetting(params string[] settings)
        {
            var doc = GetProjectDocument(Directory.GetCurrentDirectory());
            var nsmgr = new XmlNamespaceManager(doc.NameTable);

            foreach(var setting in settings)
            {
                var xpath = $"/Project/PropertyGroup/{setting}";
                var node = doc.SelectSingleNode(xpath, nsmgr);
                if (node != null)
                {
                    return node.InnerText;
                }
            }
            return null;
        }

        private static XmlDocument GetProjectDocument(string projectFolder)
        {
            return ProjectDocuments.GetOrAdd(projectFolder, _ =>
            {
                var projectDocument = new XmlDocument();
                projectDocument.Load(GetCsProjFile().FullName);
                return projectDocument;
            });
        }

         private static void GenerateCodeFromSwagger(List<FileInfo> files)
        {
            var nonexisingFiles = files.Where(o => !o.Exists);
            if (nonexisingFiles.Any())
            {
                Console.Error.WriteLine($"Cannot find files {string.Join(",", nonexisingFiles.Select(o => o.FullName))}");
                Environment.Exit(1);
            }
            files.ForEach(o => GenerateCodeFromSwagger(o));
        }

        private static void GenerateCodeFromSwagger(FileInfo swaggerFile)
        {
            using (var fr = swaggerFile.OpenText())
            {
                var swaggerSpec = fr.ReadToEnd();
                var settings = new OpenApiCodeSettings()
                {
                    SwaggerSpec = swaggerSpec,
                    OutputPath = swaggerFile.DirectoryName,
                    ProjectNamespace = GetProjectNamespace()
                };

                OpenApiCodeGenerator.GenerateCode(settings);
            }
        }

        private static FileInfo GetCsProjFile()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            // locate csproj file
            var csprojFiles = dir.GetFiles("*.csproj");
            if (csprojFiles.Length != 1)
            {
                throw new Exception("Cannot find csproj file in folder:" + dir.FullName);
            }
            return csprojFiles.First();
        }

        private static string GetProjectNamespace()
        {
            var csProjFile = GetCsProjFile();
            var projectNamespace = csProjFile.Name;
            projectNamespace = projectNamespace.Substring(0, projectNamespace.Length - csProjFile.Extension.Length);
            return projectNamespace;
        }

        private static string GetAssemblyName()
        {
            var csProjFile = GetCsProjFile();
            var assemblyName = csProjFile.Name;
            assemblyName = assemblyName.Substring(0, assemblyName.Length - csProjFile.Extension.Length);
            return assemblyName;
        }
    }
}
