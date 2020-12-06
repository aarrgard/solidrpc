﻿using SolidRpc.OpenApi.AzFunctions.Functions;
using System.Text;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Represents a function
    /// </summary>
    public class FunctionDef
    {
        public FunctionDef(IAzFunctionHandler functionHandler, string protocol, string openApiPath, string path)
        {
            FunctionHandler = functionHandler;
            Protocol = protocol;
            FunctionName = CreateFunctionName(openApiPath);
            var (pathWithNoArgNames, pathWithArgNames) = FixupPath(path);
            Path = pathWithNoArgNames;
            PathWithArgNames = pathWithArgNames;
        }

        private (string, string) FixupPath(string path)
        {
            // remove frontend prefix
            if (path.StartsWith($"{FunctionHandler.HttpRouteFrontendPrefix}/"))
            {
                path = path.Substring(FunctionHandler.HttpRouteFrontendPrefix.Length + 1);
            }
            //
            // transform wildcard names
            //
            var level = 0;
            var pathWithNoArgNames = new StringBuilder();
            var pathWithArgNames = new StringBuilder();
            var argCount = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '}')
                {
                    level--;
                }
                if (level == 0)
                {
                    pathWithNoArgNames.Append(path[i]);
                    pathWithArgNames.Append(path[i]);
                }
                if (path[i] == '{')
                {
                    if (level == 0)
                    {
                        pathWithArgNames.Append("arg").Append(argCount);
                        argCount++;
                    }
                    level++;
                }
            }

            return (pathWithNoArgNames.ToString(), pathWithArgNames.ToString());
        }
        private string CreateFunctionName(string functionName)
        {
            int argCount = 0;
            var sb = new StringBuilder();
            sb.Append(Protocol.Substring(0, 1).ToUpper());
            sb.Append(Protocol.Substring(1).ToLower());
            int level = 0;
            foreach (var c in functionName)
            {
                switch (c)
                {
                    case '{':
                        sb.Append($"arg{argCount++}");
                        level++;
                        break;
                    case '}':
                        level--;
                        break;
                    case '.':
                    case '/':
                        sb.Append('_');
                        break;
                    default:
                        if (level == 0)
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        public IAzFunctionHandler FunctionHandler { get; }
        public string Protocol { get; }
        public string Path { get; }
        public string PathWithArgNames { get; }
        public string FunctionName { get; }
    }

    public class HttpFunctionDef : FunctionDef
    {
        public HttpFunctionDef(IAzFunctionHandler functionHandler, string protocol, string openApiPath, string path) : base(functionHandler, protocol, openApiPath, path) { }

        public string Method { get; set; }
        public string AuthLevel { get; set; }
    }

    public class QueueFunctionDef : FunctionDef
    {
        public QueueFunctionDef(IAzFunctionHandler functionHandler, string protocol, string openApiPath, string path) : base(functionHandler, protocol, openApiPath, path) { }
        public string QueueName { get; set; }
        public string Connection { get; set; }
        public string TransportType { get; set; }
    }

}
