using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SolidProxy.Core.Configuration;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Model.CodeDoc;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodBindingV2 : IMethodBinding
    {
    
        private ConcurrentDictionary<Type, Func<object, Task>> s_TaskFactory = new ConcurrentDictionary<Type, Func<object, Task>>();
        public static ParameterObject GetParameterObject(OperationObject operationObject, ParameterInfo parameterInfo)
        {
            var parameters = operationObject.GetParameters();
            var parameter = parameters.FirstOrDefault(o => NameMatches(o.Name, parameterInfo.Name));
            if(parameter == null)
            {
                if (parameterInfo.ParameterType == typeof(CancellationToken))
                {
                    return new ParameterObject(operationObject)
                    {
                        In = "skip",
                        Name = parameterInfo.Name
                    };
                }
                if (parameterInfo.ParameterType == typeof(IPrincipal))
                {
                    return new ParameterObject(operationObject)
                    {
                        In = "skip",
                        Name = parameterInfo.Name
                    };
                }
                if (parameterInfo.ParameterType.IsHttpRequest())
                {
                    return new ParameterObject(operationObject)
                    {
                        In = "request",
                        Name = parameterInfo.Name
                    };
                }
                if (operationObject.Parameters.Any(o => o.IsBinaryType()))
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
                    if (bodyParam.Name == "body") return bodyParam;
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

        public static bool NameMatches(string openApiName, string name)
        {
            int openApiNameIdx = 0;
            while (openApiNameIdx < openApiName.Length)
            {
                while (openApiNameIdx < openApiName.Length && SolidRpcConstants.OpenApiWordSeparators.Contains(openApiName[openApiNameIdx])) openApiNameIdx++;
                if (NameMatchesInternal(openApiNameIdx, openApiName, name))
                {
                    return true;
                }
                while (openApiNameIdx < openApiName.Length && !SolidRpcConstants.OpenApiWordSeparators.Contains(openApiName[openApiNameIdx])) openApiNameIdx++;
            }
            return false;
        }

        private static bool NameMatchesInternal(int name1Idx, string name1, string name2)
        {
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
                        while (name2Idx < name2.Length && SolidRpcConstants.OpenApiWordSeparators.Contains(name2[name2Idx])) name2Idx++;
                        return name2.Length == name2Idx;
                    }
                    if (name2.Length <= name2Idx)
                    {
                        if (name1.Length > name1Idx)
                        {
                            if(SolidRpcConstants.OpenApiWordSeparators.Contains(name1[name1Idx]))
                            {
                                return true;
                            }
                            return false;
                        }
                        return true;
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
            IEnumerable<ITransport> transports)
        {
            MethodBinder = methodBinder ?? throw new ArgumentNullException(nameof(methodBinder));
            CodeDocMethod = codeDocMethod ?? throw new ArgumentNullException(nameof(codeDocMethod));
            OperationObject = operationObject ?? throw new ArgumentNullException(nameof(operationObject));
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            Transports = transports.OrderBy(o => o.GetInvocationOrdinal()).ToArray();
            SerializerFactory = MethodBinder.ServiceProvider.GetRequiredService<ISerializerFactory>();
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
                if (_exceptionMappings == null)
                {
                    // extract exception types
                    _exceptionMappings = new Dictionary<int, Action>();
                    _exceptionMappings[UnauthorizedException.HttpStatusCode] = CreateExceptionThrower(typeof(UnauthorizedException));
                    _exceptionMappings[FileContentNotFoundException.HttpStatusCode] = CreateExceptionThrower(typeof(FileContentNotFoundException));
                    _exceptionMappings[RateLimitExceededException.HttpStatusCode] = CreateExceptionThrower(typeof(RateLimitExceededException));
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
        private Uri _operationAddress;
        public Uri OperationAddress {
            get
            {
                if (_operationAddress == null)
                {
                    _operationAddress = OperationObject.GetAddress();

                }
                return _operationAddress;
            }
         }

        private bool? _isLocal;
        public bool IsLocal
        {
            get
            {
                if (!_isLocal.HasValue)
                {
                    var proxy = (ISolidProxy)MethodBinder.ServiceProvider.GetService(MethodInfo.DeclaringType);
                    var advicePipeline = proxy.GetInvocationAdvices(MethodInfo);
                    _isLocal = advicePipeline.OfType<SolidProxyInvocationImplAdvice>().Any();
                }
                return _isLocal.Value;
            }
        }

        private bool? _isEnabled;
        public bool IsEnabled
        {
            get
            {
                if (!_isEnabled.HasValue)
                {
                    var proxy = (ISolidProxy)MethodBinder.ServiceProvider.GetService(MethodInfo.DeclaringType);
                    var invocationConfig = proxy.GetInvocations().Where(o => o.SolidProxyInvocationConfiguration.MethodInfo == MethodInfo).Single().SolidProxyInvocationConfiguration;
                    if (!invocationConfig.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                    {
                        _isEnabled = false;
                    }
                    else
                    {
                        _isEnabled = invocationConfig.ConfigureAdvice<ISolidRpcOpenApiConfig>().Enabled;
                    }
                }
                return _isEnabled.Value;
            }
        }

        public string Path => OperationObject.GetPath()?.Substring(1);

        public Uri BindUri(IHttpRequest request)
        {
            var ub = new UriBuilder();
            ub.Scheme = request.Scheme;
            ub.Host = request.GetHost();
            var port = request.GetPort();
            if(port != null)
            {
                ub.Port = port.Value;
            }
            ub.Path = request.Path;
            ub.Query = string.Join("&",request.Query.Select(o => $"{o.Name}={HttpUtility.UrlEncode(o.GetStringValue())}"));
            return ub.Uri;
        }

        public Task BindArgumentsAsync(IHttpRequest request, object[] args, Uri addressOverride)
        {
            return BindArgumentsAsync(request, args, addressOverride, new[] { "path", "query" });
        }

        private async Task BindArgumentsAsync(IHttpRequest request, object[] args, Uri addressOverride, IEnumerable<string> locations)
        {
            if (args.Length != Arguments.Length)
            {
                throw new ArgumentException($"Number of supplied arguments({args.Length}) does not match number of arguments in method({Arguments.Length}).");
            }
            if (addressOverride == null)
            {
                addressOverride = OperationAddress;
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
            request.Scheme = addressOverride.Scheme;
            if (addressOverride.IsDefaultPort)
            {
                request.HostAndPort = addressOverride.Host;
            }
            else
            {
                request.HostAndPort = $"{addressOverride.Host}:{addressOverride.Port}";
            }
            request.Path = addressOverride.LocalPath;

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
            return (T)ExtractResponse(typeof(T), response);
        }
        public object ExtractResponse(Type responseType, IHttpResponse response)
        {
            if(responseType.IsTaskType(out Type taskType))
            {
                if (taskType == null) return Task.CompletedTask;
                var res = ExtractResponse(taskType, response);
                return s_TaskFactory.GetOrAdd(taskType, CreateTaskFactory)(res);
            }
            var validResponses = new int[] { 200, 204, 302 };
            if (!validResponses.Contains(response.StatusCode))
            {
                Action exceptionAction;
                if(ExceptionMappings.TryGetValue(response.StatusCode, out exceptionAction))
                {
                    exceptionAction();
                }
                throw new Exception("Status:"+response.StatusCode);
            }
            var template = FileContentTemplate.GetTemplate(responseType);
            if (template.IsTemplateType)
            {
                object res;
                if(typeof(Stream).IsAssignableFrom(responseType))
                {
                    res = new MemoryStream();
                }
                else
                {
                    res = Activator.CreateInstance(responseType);
                }
                template.SetContent(res, response.ResponseStream);
                template.SetContentType(res, response.MediaType);
                template.SetCharSet(res, response.CharSet);
                template.SetFileName(res, response.Filename);
                template.SetLastModified(res, response.LastModified);
                template.SetLocation(res, response.Location);
                template.SetETag(res, response.ETag);

                if (string.IsNullOrEmpty(response.MediaType) && string.IsNullOrEmpty(response.Location))
                {
                    return null;
                }

                return res;
            }
            if (Produces.Any())
            {
                if (!Produces.Any(o => ContentTypeMatches(o, response.MediaType)))
                {
                    throw new Exception($"Operation does not support content type {response.MediaType}. Supported content types are {string.Join(",", Produces)}");
                }
            }
            switch (response.MediaType?.ToLower())
            {
                case null:
                    if(responseType.IsValueType)
                    {
                        return Activator.CreateInstance(responseType);
                    }
                    else
                    {
                        return null;
                    }
                case "text/javascript":
                case "application/json":
                    using (var s = response.ResponseStream)
                    {
                        var respContent = JsonHelper.Deserialize(s, responseType);
                        return respContent;
                    }
            }
            throw new Exception("Cannot handle content type:" + response.MediaType);
        }

        private Func<object, Task> CreateTaskFactory(Type type)
        {
            var gmi = typeof(Task).GetMethods()
                .Where(o => o.Name == nameof(Task.FromResult))
                .Single();
            gmi = gmi.MakeGenericMethod(type);
            return _ => (Task)gmi.Invoke(null, new object[] { _ });
        }

        private string RemoveQuotes(string str)
        {
            if (str == null) return null;
            if (!str.StartsWith("\"")) return str;
            if (!str.EndsWith("\"")) return str;
            return str.Substring(1, str.Length - 2);
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

        public IEnumerable<ITransport> Transports { get; }

        private ISerializerFactory SerializerFactory { get; }

        public string LocalPath => OperationObject.GetAbsolutePath();

        public string RelativePath => OperationObject.GetPath().Substring(1);

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
                if (respCode != null)
                {
                    response.StatusCode = respCode.Value;
                }
                var httpLocation = ex.Data["HttpLocation"] as Uri;
                if (httpLocation != null)
                {
                    response.Location = httpLocation.ToString();
                }
                return Task.CompletedTask;
            }

            if(objType == typeof(void))
            {
                return Task.CompletedTask;
            }

            //
            // map return types
            //
            var fileTemplate = FileContentTemplate.GetTemplate(objType);
            if (fileTemplate.IsTemplateType)
            {
                if(obj == null)
                {
                    // nothinh to resturn - make sure that the content type is empty
                    response.MediaType = null;
                    return Task.CompletedTask;
                }
                response.ResponseStream = fileTemplate.GetContent(obj);
                response.MediaType = fileTemplate.GetContentType(obj);
                if(response.ResponseStream != null && response.MediaType == null)
                {
                    response.MediaType = "application/octet-stream";
                }
                response.CharSet = fileTemplate.GetCharSet(obj);
                response.Filename = fileTemplate.GetFileName(obj);
                response.LastModified = fileTemplate.GetLastModified(obj);
                response.Location = fileTemplate.GetLocation(obj);
                response.ETag = fileTemplate.GetETag(obj);
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
                    response.ResponseStream = JsonHelper.Serialize(obj, objType);
                    response.MediaType = "application/json";
                    break;
                default:
                    throw new Exception("Cannot handle content type:" + contentType);
            }

            return Task.CompletedTask;
        }

        public T GetSolidProxyConfig<T>() where T : class, ISolidProxyInvocationAdviceConfig
        {
            return MethodBinder.GetSolidProxyConfig<T>(MethodInfo);
        }
    }
}
