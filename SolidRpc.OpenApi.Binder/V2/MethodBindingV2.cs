using Newtonsoft.Json;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Model.CodeDoc;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodBindingV2 : IMethodBinding
    {
        public static ParameterObject GetParameterObject(OperationObject operationObject, ParameterInfo parameterInfo)
        {
            var parameters = operationObject.GetParameters();
            var parameter = parameters.FirstOrDefault(o => NameMatches(o.Name, parameterInfo.Name));
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
                var bodyParam = parameters.FirstOrDefault(o => o.In == "body");
                if (bodyParam != null)
                {
                    var schema = bodyParam.Schema.GetRefSchema() ?? bodyParam.Schema;
                    if (schema != null)
                    {
                        if (schema.GetProperties().ContainsKey(parameterInfo.Name))
                        {
                            return bodyParam;
                        }
                    }
                }
                throw new Exception($"Cannot find parameter {parameterInfo.Name} among parameters({string.Join(",",parameters.Select(o => o.Name))}).");
            }
            return parameter;
        }

        public static bool NameMatches(string name1, string name2)
        {
            int name1Idx = 0;
            int name2Idx = 0;
            while(true)
            {
                while (name1Idx < name1.Length && SolidRpcConstants.OpenApiWordSeparators.Contains(name1[name1Idx])) name1Idx++;
                while (name2Idx < name2.Length && SolidRpcConstants.OpenApiWordSeparators.Contains(name2[name2Idx])) name2Idx++;

                if (name1.Length == name1Idx && name2.Length == name2Idx)
                {
                    return true;
                }
                if (name1.Length <= name1Idx)
                {
                    return false;
                }
                if (name2.Length <= name2Idx)
                {
                    return false;
                }
                if (char.ToLower(name1[name1Idx]) != char.ToLower(name2[name2Idx]))
                {
                    return false;
                }
                while (true)
                {
                    if (name1.Length == name1Idx && name2.Length == name2Idx)
                    {
                        return true;
                    }
                    if (name1.Length <= name1Idx)
                    {
                        return false;
                    }
                    if (name2.Length <= name2Idx)
                    {
                        return false;
                    }
                    if (char.ToLower(name1[name1Idx]) == char.ToLower(name2[name2Idx]))
                    {
                        name1Idx++;
                        name2Idx++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public MethodBindingV2(
            MethodBinderV2 methodBinder, 
            OperationObject operationObject, 
            MethodInfo methodInfo, 
            ICodeDocMethod codeDocMethod,
            MethodAddressTransformer methodAddressTransformer)
        {
            MethodBinder = methodBinder ?? throw new ArgumentNullException(nameof(methodBinder));
            CodeDocMethod = codeDocMethod ?? throw new ArgumentNullException(nameof(codeDocMethod));
            OperationObject = operationObject ?? throw new ArgumentNullException(nameof(operationObject));
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            MethodAddressTransformer = methodAddressTransformer ?? throw new ArgumentNullException(nameof(methodInfo));
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

        public ICodeDocMethod CodeDocMethod { get; }

        public OperationObject OperationObject { get; }

        public MethodInfo MethodInfo { get; }

        private MethodAddressTransformer MethodAddressTransformer { get; }

        private IMethodArgument[] _arguments;
        public IMethodArgument[] Arguments
        {
            get
            {
                if(_arguments == null)
                {
                    _arguments = MethodInfo.GetParameters().Select(parameterInfo =>
                    {
                        var parameterObject = GetParameterObject(OperationObject, parameterInfo);
                        return new MethodArgument(parameterObject, parameterInfo);
                    }).ToArray();
                }
                return _arguments;
            }
        }

        IEnumerable<IMethodArgument> IMethodBinding.Arguments => Arguments;

        private IDictionary<int, Action> _exceptionMappings;
        public IDictionary<int, Action> ExceptionMappings
        {
            get
            {
                if(_exceptionMappings == null)
                {
                    // extract exception types
                    _exceptionMappings = new Dictionary<int, Action>();
                    CodeDocMethod.ExceptionDocumentation.ToList().ForEach(o =>
                    {
                        var exceptionType = MethodInfo.DeclaringType.Assembly.GetType(o.ExceptionType);
                        var exceptionInstance = (Exception)Activator.CreateInstance(exceptionType);
                        var httpStatusCode = exceptionInstance.Data["HttpStatusCode"] as int?;
                        if (httpStatusCode != null)
                        {
                            ExceptionMappings[httpStatusCode.Value] = CreateExceptionThrower(exceptionType);
                        }
                    });
                }
                return _exceptionMappings;
            }
        }

        public MethodBinderV2 MethodBinder { get; }
        IMethodBinder IMethodBinding.MethodBinder => MethodBinder;

        public string OperationId => OperationObject.OperationId;

        private string _method;
        public string Method
        {
            get
            {
                if(_method == null)
                {
                    _method = OperationObject.GetMethod();
                }
                return _method;
            }
        }

        private string _scheme;
        public string Scheme
        {
            get
            {
                if(_scheme == null)
                {
                    _scheme = OperationObject.Schemes?.FirstOrDefault() ??
                        OperationObject.GetParent<SwaggerObject>().Schemes?.FirstOrDefault() ??
                        "http";
                }
                return _scheme;
            }
        }

        private IEnumerable<string> _produces;
        public IEnumerable<string> Produces
        {
            get
            {
                if(_produces == null)
                {
                    _produces = OperationObject.GetProduces();
                }
                return _produces;
            }
        }
        private Uri _address;
        public Uri Address {
            get
            {
                if (_address == null)
                {
                    var address = OperationObject.GetAddress();
                    if(MethodAddressTransformer != null)
                    {
                        try
                        {
                            address = MethodAddressTransformer(MethodBinder.ServiceProvider, address, MethodInfo).Result;
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                    _address = address;

                }
                return _address;
            }
            set
            {
                _address = value ?? throw new ArgumentNullException();
            }
        }

        private bool? _isLocal;
        public bool IsLocal
        {
            get
            {
                if(!_isLocal.HasValue)
                {
                    var proxy = (ISolidProxy)MethodBinder.ServiceProvider.GetService(MethodInfo.DeclaringType);
                    var advicePipeline = proxy.GetInvocationAdvices(MethodInfo);
                    _isLocal = advicePipeline.OfType<ISolidProxyInvocationAdvice>().Any();
                }
                return _isLocal.Value;
            }
        }

        public string Path => OperationObject.GetPath()?.Substring(1);

        public async Task BindArgumentsAsync(IHttpRequest request, object[] args)
        {
            if(args.Length != Arguments.Length)
            {
                throw new ArgumentException($"Number of supplied arguments({args.Length}) does not match number of arguments in method({Arguments.Length}).");
            }


            //
            // bind arguments
            //
            for (int i = 0; i < Arguments.Length; i++)
            {
                await Arguments[i].BindArgumentAsync(request, args[i]);
            }

            //
            // set path
            // 
            request.Method = Method;
            request.Scheme = Address.Scheme;
            if (Address.IsDefaultPort)
            {
                request.HostAndPort = Address.Host;
            }
            else
            {
                request.HostAndPort = $"{Address.Host}:{Address.Port}";
            }
            request.Path = Address.LocalPath;

            foreach (var pathData in request.PathData)
            {
                request.Path = request.Path.Replace($"{{{pathData.Name}}}", HttpUtility.UrlEncode(pathData.GetStringValue()));
            }

            //
            // set content type
            //
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
                if (request.BodyData.All(o => o.GetType() == typeof(SolidHttpRequestDataString)))
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
            var contentTypes = request.BodyData.Select(o => o.ContentType).Distinct();
            if (contentTypes.Count() > 1)
            {
                throw new Exception("Cannot determine content type");
            }
            return contentTypes.First();
        }

        public T ExtractResponse<T>(IHttpResponse response)
        {
            if(response.StatusCode != 200)
            {
                Action exceptionAction;
                if(ExceptionMappings.TryGetValue(response.StatusCode, out exceptionAction))
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
                if (!Produces.Any(o => ContentTypeMatches(o, response.ContentType)))
                {
                    throw new Exception($"Operation does not support content type {response.ContentType}. Supported content types are {string.Join(",", Produces)}");
                }
            }
            switch (response.ContentType.ToLower())
            {
                case "text/javascript":
                case "application/json":
                    using (var s = response.ResponseStream)
                    {
                        return JsonHelper.Deserialize<T>(s);
                    }
            }
            if(typeof(T).IsFileType())
            {
                var res = Activator.CreateInstance<T>();
                typeof(T).SetFileTypeStreamData(res, response.ResponseStream);
                typeof(T).SetFileTypeContentType(res, response.ContentType);
                typeof(T).SetFileTypeFilename(res, response.Filename);
                typeof(T).SetFileTypeLastModified(res, response.LastModified);
                typeof(T).SetFileTypeLocation(res, response.Location);
                return res;
            }
            throw new Exception("Cannot handle content type:" + response.ContentType);
        }

        private bool ContentTypeMatches(string contentTypePattern, string contentType)
        {
            var contentTypePatternParts = (contentTypePattern ?? "").Split('/');
            var contentTypeParts = (contentTypePattern ?? "").Split('/');
            if(contentTypePatternParts.Length != contentTypeParts.Length)
            {
                return false;
            }
            for(int i = 0; i < contentTypePatternParts.Length; i++)
            {
                if (contentTypePatternParts[i].Equals("*"))
                {
                    continue;
                }
                if (contentTypePatternParts[i].Equals(contentTypeParts[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        private IList<string> _patterns;
        public IList<string> Patterns
        {
            get
            {
                if(_patterns == null)
                {

                    var path = OperationObject.GetAddress().LocalPath;
                    var patterns = path.Split('/');
                    for(int i = 0; i < patterns.Length; i++)
                    {
                        if (!patterns[i].StartsWith("{"))
                        {
                            patterns[i] = null;
                            continue;
                        }
                        if (!patterns[i].EndsWith("}"))
                        {
                            patterns[i] = null;
                            continue;
                        }
                        patterns[i] = patterns[i].Substring(1, patterns[i].Length - 2);
                    }
                    _patterns = patterns;
                }
                return _patterns;
            }
        }

        public async Task<object[]> ExtractArgumentsAsync(IHttpRequest request)
        {
            // extract path data
            ExtractPathData(request);

            // extract body parameters
            ExtractBodyJsonParameters(request);

            // then bind the arguments
            var args = new object[Arguments.Length];
            for(int i =0; i < args.Length; i++)
            {
                args[i] = await Arguments[i].ExtractArgumentAsync(request);
            }
            return args;
        }

        private void ExtractPathData(IHttpRequest request)
        {
            // create path data
            var patterns = Patterns;
            var pathElements = request.Path.Split('/');
            if (patterns.Count != pathElements.Length)
            {
                throw new Exception($"Supplied request path({request.Path}) does not match operation path({OperationObject.GetPath()})");
            }
            var pathData = new List<SolidHttpRequestData>();
            for (int i = 0; i < patterns.Count; i++)
            {
                if (patterns[i] == null) continue;
                var data = HttpUtility.UrlDecode(pathElements[i]);
                pathData.Add(new SolidHttpRequestDataString("text/plain", patterns[i], data));
            }

            request.PathData = pathData;
        }

        private void ExtractBodyJsonParameters(IHttpRequest request)
        {
            if(request.BodyData.Count() != 1)
            {
                return;
            }
            var bodyArguments = Arguments.Where(o => o.Location == "body");
            if (bodyArguments.Count() < 2)
            {
                return;
            }
            var bodyData = request.BodyData.First();
            var contents = ExtractJsonContents(bodyData);
            request.BodyData = contents.Select(o => new SolidHttpRequestDataString(bodyData.ContentType, o.Key, o.Value)).ToArray();
        }

        private IDictionary<string, string> ExtractJsonContents(IHttpRequestData bodyData)
        {
            var res = new Dictionary<string, string>();
            using (var s = bodyData.GetBinaryValue())
            using (var tr = new StreamReader(s, bodyData.Encoding ?? Encoding.UTF8))
            using (var jr = new JsonTextReader(tr))
            {
                while(jr.Read())
                {
                    if(jr.TokenType == JsonToken.PropertyName)
                    {
                        var propertyName = (string)jr.Value;
                        jr.Read();
                        var sw = new StringWriter();
                        var jw = new JsonTextWriter(sw);
                        jw.WriteToken(jr);
                        jw.Close();
                        sw.Close();
                        res[propertyName] = sw.ToString();
                    }
                }
            }
            return res;
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
            if (objType.IsFileType())
            {
                var charSet = objType.GetFileTypeCharSet(obj);
                var retContentType = objType.GetFileTypeContentType(obj);
                if(!string.IsNullOrEmpty(charSet))
                {
                    retContentType = $"{retContentType}; charset=\"{charSet}\"";
                }
                response.ContentType = retContentType;
                response.ResponseStream = objType.GetFileTypeStreamData(obj);
                response.Filename = objType.GetFileTypeFilename(obj);
                response.LastModified = objType.GetFileTypeLastModified(obj);
                response.Location = objType.GetFileTypeLocation(obj);
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

        private Func<object, Task<object>> CreateExtractor<T>()
        {
            return async (o) => await (Task<T>)o;
        }
    }
}
