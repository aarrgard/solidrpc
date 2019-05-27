using System;
using System.Linq;

namespace SolidRpc.Swagger.DotNetTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running swagger-generator");
            args.ToList().ForEach(o =>
            {
                Console.WriteLine($" * arg:{o}");
            });
        }
    }
}
