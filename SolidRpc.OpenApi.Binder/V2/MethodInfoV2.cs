using SolidRpc.OpenApi.Model.CodeDoc;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodInfoV2 : IMethodInfo
    {
        public static ParameterObject GetParameterObject(OperationObject operationObject, ParameterInfo parameterInfo)
        {
            var parameters = operationObject.GetParameters()
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
                if(operationObject.Parameters.Any(o => o.IsBinaryType()))
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

        public MethodInfoV2(OperationObject operationObject, MethodInfo methodInfo, ICodeDocMethod codeDocMethod)
        {
            CodeDocMethod = codeDocMethod ?? throw new ArgumentNullException(nameof(codeDocMethod));
            OperationObject = operationObject ?? throw new ArgumentNullException(nameof(operationObject));
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            Arguments = CreateArguments();
            OperationId = OperationObject.OperationId;
            Method = OperationObject.GetMethod();
            Scheme = OperationObject.Schemes?.FirstOrDefault() ??
                OperationObject.GetParent<SwaggerObject>().Schemes?.FirstOrDefault() ??
                "http";
            Host = OperationObject.GetParent<SwaggerObject>().Host;
            Path = OperationObject.GetAbsolutePath();
            Produces = OperationObject.GetProduces();

            // extract exception types
            ExcpetionMappings = new Dictionary<int, Action>();
            codeDocMethod.ExceptionDocumentation.ToList().ForEach(o =>
            {
                var exceptionType = MethodInfo.DeclaringType.Assembly.GetType(o.ExceptionType);
                var exceptionInstance = (Exception)Activator.CreateInstance(exceptionType);
                var httpStatusCode = exceptionInstance.Data["HttpStatusCode"] as int?;
                if (httpStatusCode != null)
                {
                    ExcpetionMappings[httpStatusCode.Value] = CreateExceptionThrower(exceptionType);
                }
            });
        }

        private Action CreateExceptionThrower(Type exceptionType)
        {
            var lamda = Expression.Lambda(
                    Expression.Throw(
                        Expression.Constant(Activator.CreateInstance(exceptionType))
                    ),
                    new ParameterExpression[0]
                );
            return (Action)lamda.Compile();
        }
         public void Analyze(Expression<Action> expr)
        {

        } 

        private IMethodArgument[] CreateArguments()
        {
            return MethodInfo.GetParameters().Select(parameterInfo => {
                var parameterObject = GetParameterObject(OperationObject, parameterInfo);
                return new MethodArgument(parameterObject, parameterInfo);
            }).ToArray();
        }

        public ICodeDocMethod CodeDocMethod { get; }

        public OperationObject OperationObject { get; }

        public MethodInfo MethodInfo { get; }

        public IMethodArgument[] Arguments { get; }

        IEnumerable<IMethodArgument> IMethodInfo.Arguments => Arguments;
        public IDictionary<int, Action> ExcpetionMappings { get; }

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

            foreach(var pathData in request.PathData)
            {
                request.Path = request.Path.Replace($"{{{pathData.Name}}}", pathData.GetStringValue());
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
            if(OperationObject.Parameters.Any(o => o.IsBinaryType()))
            {
                return "multipart/form-data";
            }
            if (request.BodyData.Count() == 1)
            {
                return request.BodyData.First().ContentType;
            }
            return "multipart/form-data";
        }

        public T ExtractResponse<T>(IHttpResponse response)
        {
            if(response.StatusCode != 200)
            {
                Action exceptionAction;
                if(ExcpetionMappings.TryGetValue(response.StatusCode, out exceptionAction))
                {
                    exceptionAction();
                }
                throw new Exception("Status:"+response.StatusCode);
            }
            if(typeof(T).IsAssignableFrom(typeof(Stream)))
            {
                return (T)(object)response.ResponseStream;
            }
            if(Produces.Any())
            {
                if (!Produces.Contains(response.ContentType))
                {
                    throw new Exception($"Operation does not support content type {response.ContentType}. Supported content types are {string.Join(",", Produces)}");
                }
            }
            switch (response.ContentType.ToLower())
            {
                case "application/json":
                    using (var s = response.ResponseStream)
                    {
                        return JsonHelper.Deserialize<T>(s);
                    }
                default:
                    throw new Exception("Cannot handle content type:"+response.ContentType);
                    
            }
        }

        public async Task<object[]> ExtractArgumentsAsync(IHttpRequest request)
        {
            // create path data
            var patterns = OperationObject.GetAbsolutePath().Split('/');
            var pathElements = request.Path.Split('/');
            if(patterns.Length != pathElements.Length)
            {
                throw new Exception($"Supplied request path({request.Path}) does not match operation path({OperationObject.GetAbsolutePath()})");
            }
            var pathData = new List<HttpRequestData>();
            for(int i = 0; i < patterns.Length; i++)
            {
                var pattern = patterns[i];
                if(pattern.StartsWith("{") && pattern.EndsWith("}"))
                {
                    pathData.Add(new HttpRequestDataString("text/plain", pattern.Substring(1, pattern.Length - 2), pathElements[i]));
                }
            }
            request.PathData = pathData;

            // then bind the arguments
            var args = new object[Arguments.Length];
            for(int i =0; i < args.Length; i++)
            {
                args[i] = await Arguments[i].ExtractArgumentAsync(request);
            }
            return args;
        }

        public Task BindResponseAsync(IHttpResponse response, object obj, Type objType)
        {
            response.StatusCode = 200;

            //
            // handle exception responses
            //
            if(obj is Exception ex)
            {
                response.StatusCode = 500;
                var respCode = ex.Data["HttpStatusCode"] as int?;
                if(respCode != null)
                {
                    response.StatusCode = respCode.Value;
                }
                return Task.CompletedTask;
            }

            //
            // map return types
            //
            var returnType = MethodInfo.ReturnType;
            if (returnType.IsFileType())
            {
                response.ContentType = returnType.GetFileTypeContentType(obj);
                response.ResponseStream = returnType.GetFileTypeStreamData(obj);
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
                    response.ResponseStream = JsonHelper.Serialize(obj, objType);
                    break;
                default:
                    throw new Exception("Cannot handle content type:" + contentType);
            }

            return Task.CompletedTask;
        }
    }
}
