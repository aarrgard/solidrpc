using SolidRpc.Swagger.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.Swagger.DotNetTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running swagger-generator");
            var argList = new List<string>(args);
            var command = argList.Where(o => o == "--code2swagger" || o == "--swagger2code").Single();
            argList.Remove(command);

            var workingDir = Directory.GetCurrentDirectory();
            var files = argList.Select(o => new FileInfo(Path.Combine(workingDir, o))).ToList();
            var nonexisingFiles = files.Where(o => !o.Exists);
            if(nonexisingFiles.Any())
            {
                Console.Error.WriteLine($"Cannot find files {string.Join(",", nonexisingFiles.Select(o => o.FullName))}");
                Environment.Exit(1);
            }

            switch(command)
            {
                case "--code2swagger":
                    break;
                case "--swagger2code":
                    files.ForEach(o => GenerateCode(o));
                    break;
            }
        }

        private static void GenerateCode(FileInfo swaggerFile)
        {
            using (var fr = swaggerFile.OpenText())
            {
                var swaggerSpec = fr.ReadToEnd();
                var dirName = (new DirectoryInfo(swaggerFile.DirectoryName)).Name;
                var settings = new SwaggerCodeSettings()
                {
                    SwaggerSpec = swaggerSpec,
                    OutputPath = swaggerFile.DirectoryName,
                    ProjectNamespace = dirName
                };

                SwaggerCodeGenerator.GenerateCode(settings);
            }
        }
    }
}
