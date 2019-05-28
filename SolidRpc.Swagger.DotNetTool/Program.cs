using SolidRpc.Swagger.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SolidRpc.Swagger.DotNetTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running swagger-generator");
            var argList = new List<string>(args);
            var command = argList.Where(o => o == "--code2swagger" || o == "--swagger2code").SingleOrDefault();
            if(command == null)
            {
                Console.Error.WriteLine($"Must supply direction of code: --code2swagger or --swagger2code");
                Environment.Exit(1);
            }
            argList.Remove(command);

            var workingDir = Directory.GetCurrentDirectory();
            var files = argList.Select(o => new FileInfo(Path.Combine(workingDir, o))).ToList();

            switch(command)
            {
                case "--code2swagger":
                    GenerateSwaggerFromCode(files);
                    break;
                case "--swagger2code":
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
            // find the project assembly.
            //var assembly = FindAssembly(fileInfo.DirectoryName);
            var settings = new SwaggerSpecSettings()
            {
                SwaggerFile = fileInfo.FullName,
                CodePath = fileInfo.DirectoryName
            };
            SwaggerSpecGenerator.GenerateCode(settings);
        }

        private static Assembly FindAssembly(string projectFolder)
        {
            var assemblyName = GetAssemblyName(projectFolder);
            var di = new DirectoryInfo(Path.Combine(projectFolder, "bin"));
            var assemblyFiles = di.GetFiles($"{assemblyName}.dll", new EnumerationOptions() { RecurseSubdirectories = true });
            var assemblyFile = assemblyFiles.OrderByDescending(o => o.LastWriteTime).FirstOrDefault();
            if(assemblyFile == null)
            {
                Console.Error.WriteLine($"Cannot locate assembly {assemblyName}.dll in or below folder {di.FullName}");
                Environment.Exit(1);
            }
            try
            {
                // load the assembly and generate swagger file
                return Assembly.LoadFile(assemblyFile.FullName);
            } 
            catch(Exception e)
            {
                Console.Error.WriteLine($"Cannot load the assembly:" + assemblyFile.FullName);
                Environment.Exit(1);
                return null;
            }
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
                var settings = new SwaggerCodeSettings()
                {
                    SwaggerSpec = swaggerSpec,
                    OutputPath = swaggerFile.DirectoryName,
                    ProjectNamespace = GetProjectNamespace(swaggerFile.DirectoryName)
                };

                SwaggerCodeGenerator.GenerateCode(settings);
            }
        }

        private static FileInfo GetCsProjFile(string projectFolder)
        {
            var dir = new DirectoryInfo(projectFolder);
            // locate csproj file
            var csprojFiles = dir.GetFiles("*.csproj");
            if (csprojFiles.Length != 1)
            {
                throw new Exception("Cannot find csproj file in folder:" + projectFolder);
            }
            return csprojFiles.First();
        }

        private static string GetProjectNamespace(string projectFolder)
        {
            var csProjFile = GetCsProjFile(projectFolder);
            var projectNamespace = csProjFile.Name;
            projectNamespace = projectNamespace.Substring(0, projectNamespace.Length - csProjFile.Extension.Length);
            return projectNamespace;
        }

        private static string GetAssemblyName(string projectFolder)
        {
            var csProjFile = GetCsProjFile(projectFolder);
            var assemblyName = csProjFile.Name;
            assemblyName = assemblyName.Substring(0, assemblyName.Length - csProjFile.Extension.Length);
            return assemblyName;
        }
    }
}
