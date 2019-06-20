using Newtonsoft.Json;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodInfoV2 : IMethodInfo
    {
        public static ParameterObject GetParameterObject(OperationObject operationObject, ParameterInfo parameterInfo)
        {
            var parameters = operationObject.Parameters
                .Where(o => o.Name == parameterInfo.Name);
            var parameter = parameters.FirstOrDefault();
            if(parameter == null)
            {
                if(parameterInfo.ParameterType == typeof(CancellationToken))
                {
                    return new ParameterObject(operationObject) {
                        In = "skip",
                        Name = parameterInfo.Name
                    };
                }
                if(operationObject.Parameters.Any(o => o.IsFileType()))
                {
                    if (parameterInfo.Name.Equals("filename", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new ParameterObject(operationObject)
                        {
                            Type = "file",
                            In = "formData",
                            Name = parameterInfo.Name
                        };
                    }
                    if (parameterInfo.Name.Equals("contenttype", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new ParameterObject(operationObject)
                        {
                            Type = "file",
                            In = "formData",
                            Name = parameterInfo.Name
                        };
                    }
                }
                throw new Exception($"Cannot find parameter {parameterInfo.Name} among parameters({string.Join(",",parameters.Select(o => o.Name))}).");
            }
            return parameter;
        }

        public MethodInfoV2(OperationObject operationObject, MethodInfo methodInfo)
        {
            OperationObject = operationObject ?? throw new ArgumentNullException(nameof(operationObject));
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            Arguments = CreateArguments();
            OperationId = OperationObject.OperationId;
            Method = OperationObject.GetMethod();
            Scheme = OperationObject.Schemes?.FirstOrDefault() ??
                OperationObject.GetParent<SwaggerObject>().Schemes?.FirstOrDefault() ??
                "http";
            Host = OperationObject.GetParent<SwaggerObject>().Host;
            Path = $"{OperationObject.GetParent<SwaggerObject>().BasePath}{OperationObject.GetPath()}";
            Produces = OperationObject.Produces ?? new string[0];
        }

        private IMethodArgument[] CreateArguments()
        {
            return MethodInfo.GetParameters().Select(parameterInfo => {
                var parameterObject = GetParameterObject(OperationObject, parameterInfo);
                return new MethodArgument(parameterObject, parameterInfo);
            }).ToArray();
        }

        public OperationObject OperationObject { get; }

        public MethodInfo MethodInfo { get; }

        public IMethodArgument[] Arguments { get; }

        IEnumerable<IMethodArgument> IMethodInfo.Arguments => Arguments;

        public string OperationId { get; }
        public string Method { get; }
        public string Scheme { get; }
        public string Host { get; }
        public string Path { get; }
        public IEnumerable<string> Produces { get; }

        public async Task BindArgumentsAsync(IHttpRequest request, object[] args)
        {
            if(args.Length != Arguments.Length)
            {
                throw new ArgumentException($"Number of supplied arguments({args.Length}) does not match number of arguments in method({Arguments.Length}).");
            }
            request.Method = Method;
            request.Scheme = Scheme;
            request.HostAndPort = Host;
            request.Path = Path;

            for (int i = 0; i < Arguments.Length; i++)
            {
                await Arguments[i].BindArgumentAsync(request, args[i]);
            }

            request.ContentType = GetContentTypeBasedOnConsumesAndData(request);
        }

        private string GetContentTypeBasedOnConsumesAndData(IHttpRequest request)
        {
            if(request.BodyData == null ||request.BodyData.Count() == 0)
            {
                return null;
            }
            var contentTypeProspects = OperationObject.GetConsumes();
            if (contentTypeProspects.Contains("application/x-www-form-urlencoded"))
            {
                if (request.BodyData.All(o => o.GetType() == typeof(HttpRequestDataString)))
                {
                    return "application/x-www-form-urlencoded";
                }
            }
            if (contentTypeProspects.Contains("multipart/form-data"))
            {
                return "multipart/form-data";
            }
            if(request.BodyData.Any(o => o is HttpRequestDataBinary))
            {
                return "multipart/form-data";
            }
            if (request.BodyData.Count() == 1)
            {
                return request.BodyData.First().ContentType;
            }
            throw new Exception("Cannot handle content type");
        }

        public T ExtractResponse<T>(IHttpResponse response)
        {
            if(response.StatusCode != 200)
            {
                throw new Exception("Status:"+response.StatusCode);
            }
            if(typeof(T).IsAssignableFrom(typeof(Stream)))
            {
                return (T)(object)response.ResponseStream;
            }
            if (!Produces.Contains(response.ContentType))
            {
                throw new Exception("Operation does not support content type:" + response.ContentType);
            }
            using (var s = response.ResponseStream)
            {
                return JsonHelper.Deserialize<T>(s);
            }
        }

        public async Task<object[]> ExtractArgumentsAsync(IHttpRequest request)
        {
            var args = new object[Arguments.Length];
            for(int i =0; i < args.Length; i++)
            {
                args[i] = await Arguments[i].ExtractArgumentAsync(request);
            }
            return args;
        }

        public Task BindResponseAsync(IHttpResponse response, object resp)
        {
            response.StatusCode = 200;
            var returnType = MethodInfo.ReturnType;
            if (returnType.IsFileType())
            {
                response.ContentType = returnType.GetFileTypeContentType(resp);
                response.ResponseStream = returnType.GetFileTypeStreamData(resp);
                return Task.CompletedTask;
            }

            string contentType = null;
            if(OperationObject.GetProduces().Contains("application/json"))
            {
                contentType = "application/json";
            }
            switch(contentType)
            {
                case null:
                case "application/json":
                    response.ContentType = "application/json";
                    using (var ms = new MemoryStream())
                    {
                        using (StreamWriter sw = new StreamWriter(ms))
                        {
                            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                            {
                                var serializer = JsonSerializer.Create();
                                serializer.Serialize(jsonWriter, resp, returnType);
                            }
                        }
                        response.ResponseStream = new MemoryStream(ms.ToArray());
                    }
                    break;
                default:
                    throw new Exception("Cannot handle content type:" + contentType);
            }

            return Task.CompletedTask;
        }
    }
}
