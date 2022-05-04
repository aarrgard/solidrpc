using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodArgument : IMethodArgument
    {
        public MethodArgument(ParameterObject parameterObject, ParameterInfo parameterInfo)
        {
            ParameterObject = parameterObject ?? throw new ArgumentNullException(nameof(parameterObject));
            ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterObject));
            ArgumentPath = GetArgumentPath();

            var contentType = "text/plain";
            if(ParameterObject.In == "body")
            {
                contentType = ParameterObject.GetParent<OperationObject>().GetConsumes().FirstOrDefault();
            } 
            else if(ParameterObject.In == "formData")
            {
                if(!SolidHttpRequestData.IsSimpleType(parameterInfo.ParameterType))
                {
                    contentType = "application/json";
                }
            }

            if (ParameterObject.IsBinaryType())
            {
                HttpRequestDataBinder = (_, __) => SetFileData(ParameterObject.Name, _, __);
                HttpRequestDataExtractor = GetFileData(contentType, ParameterObject.Name, ParameterInfo.ParameterType, ParameterObject.Format);
            }
            else if (ParameterInfo.ParameterType.IsHttpRequest())
            {
                HttpRequestDataBinder = (_, __) => { throw new Exception("This one should not be used"); };
                HttpRequestDataExtractor = (_) => { throw new Exception("This one should not be used"); };
            }
            else
            {
                var parameterName = parameterObject.Name;
                if(ParameterObject.IsBodyType())
                {
                    parameterName = ParameterInfo.Name;
                }
                string collectionFormat = ParameterObject?.CollectionFormat;
                if (collectionFormat == null)
                {
                    if(ParameterObject.Type == "array")
                    {
                        if (ParameterObject.In == "path")
                        {
                            collectionFormat = "csv";
                        }
                        else
                        {
                            collectionFormat = "multi";
                        }
                    }
                }
                HttpRequestDataBinder = SolidHttpRequestData.CreateBinder(contentType, parameterName, ParameterInfo.ParameterType, ParameterObject.Format, collectionFormat);
                HttpRequestDataExtractor = SolidHttpRequestData.CreateExtractor(contentType, parameterName, ParameterInfo.ParameterType, ParameterObject.Format, collectionFormat);
            }

            if(parameterObject.Default != null)
            {
                DefaultValue = new[] { new SolidHttpRequestDataString("text/plain", parameterObject.Name, parameterObject.Default.ToString()) { } };
            } 
            else if(parameterInfo.DefaultValue != null)
            {
                DefaultValue = new[] { new SolidHttpRequestDataString("text/plain", parameterObject.Name, parameterInfo.DefaultValue.ToString()) { } };
            }
        }

        private IEnumerable<IHttpRequestData> DefaultValue { get; }

        private Func<IEnumerable<IHttpRequestData>, object> GetFileData(string contentType, string name, Type type, string format)
        {
            var template = FileContentTemplate.GetTemplate(type);
            if(template.IsTemplateType)
            {
                return (_) =>
                {
                    var valData = _.FirstOrDefault();
                    if (valData == null) return null;
                    object data;
                    if(typeof(Stream).IsAssignableFrom(type))
                    {
                        data = new MemoryStream();
                    }
                    else
                    {
                        data = Activator.CreateInstance(type);
                    }
                    template.SetContent(data, valData.GetStreamValue());
                    template.SetContentType(data, valData.ContentType);
                    template.SetCharSet(data, valData.Encoding?.HeaderName);
                    template.SetFileName(data, valData.Filename);
                    template.SetETag(data, valData.ETag);
                    template.SetSetCookie(data, valData.SetCookie);
                    template.SetLastModified(data, valData.LastModified);
                    return data;
                };
            }
            if (type.IsAssignableFrom(typeof(Stream)))
            {
                return (_) =>
                {
                    return _.FirstOrDefault()?.GetBinaryValue();
                };
            }
            return SolidHttpRequestData.CreateExtractor(contentType, name, type, format, null);
        }

        private IEnumerable<string> GetArgumentPath()
        {
            if (ParameterObject.IsBinaryType())
            {
                var fileParameterName = ParameterObject.GetFileParameterName();
                switch (ParameterObject.Name.ToLower())
                {
                    case "filename":
                        return new[] { MapScope(ParameterObject.In), fileParameterName, "Filename" };
                    case "contenttype":
                        return new[] { MapScope(ParameterObject.In), fileParameterName, "ContentType" };

                }
            }
            if (ParameterObject.IsBodyType())
            {
                var args = ((MethodInfo)ParameterInfo.Member).GetParameters().Select(o => o.Name);
                if (ParameterObject.IsBodyTypeArgument(args))
                {
                    return new[] { MapScope(ParameterObject.In), ParameterInfo.Name };
                }
                else
                {
                    return new[] { MapScope(ParameterObject.In), "body" };
                }
            }
            if (ParameterInfo.ParameterType == typeof(CancellationToken))
            {
                return new[] { "CancellationToken" };
            }
            return new[] { MapScope(ParameterObject.In), ParameterObject.Name };
        }

        private async Task<IEnumerable<IHttpRequestData>> SetFileData(string name, IEnumerable<IHttpRequestData> formData, object value)
        {
            var latest = GetLatestBinaryData(formData);
            switch (name.ToLower())
            {
                case "contenttype":
                    latest.SetContentType((string)value);
                    break; ;
                case "filename":
                    latest.SetFilename((string)value);
                    break; ;
                default:
                    if(value is Stream stream)
                    {
                        await latest.SetBinaryData(name, stream);
                    }
                    else if(value is byte[] bytes)
                    {
                        await latest.SetBinaryData(name, new MemoryStream(bytes));
                    }
                    else
                    {
                        var template = FileContentTemplate.GetTemplate(value.GetType());
                        await latest.SetBinaryData(name, template.GetContent(value));
                        latest.SetContentType(template.GetContentType(value));
                        latest.SetCharSet(template.GetCharSet(value));
                        latest.SetFilename(template.GetFileName(value));
                        latest.SetETag(template.GetETag(value));
                        latest.SetLastModified(template.GetLastModified(value));
                    }
                    break; ;
            }
            if(formData.Contains(latest))
            {
                return formData;
            }
            return formData.Union(new[] { latest }).ToList();
        }

        private SolidHttpRequestDataBinary GetLatestBinaryData(IEnumerable<IHttpRequestData> formData)
        {
            var latest = formData.OfType<SolidHttpRequestDataBinary>()
                .Where(o => o.ContentType == "application/octet-stream").LastOrDefault();
            if(latest == null)
            {
                latest = new SolidHttpRequestDataBinary("application/octet-stream", null, "temp", (byte[])null);
                latest.SetFilename("upload.tmp");
            }
            return latest;
        }

        private string MapScope(string scope)
        {
            switch(scope)
            {
                case "skip":
                    return null;
                case "request":
                    return "";
                case "header":
                    return nameof(IHttpRequest.Headers);
                case "query":
                    return nameof(IHttpRequest.Query);
                case "path":
                    return nameof(IHttpRequest.PathData);
                case "formData":
                    return nameof(IHttpRequest.BodyData);
                case "body":
                    return nameof(IHttpRequest.BodyData);
                default:
                    throw new Exception("Cannot handle scope:" + scope);
            }
        }
        public string Location => ParameterObject.In;

        public ParameterObject ParameterObject { get; }

        public ParameterInfo ParameterInfo { get; }

        public string Name => ParameterObject.Name;

        public bool Optional => !ParameterObject.Required;

        public IEnumerable<string> ArgumentPath { get; }

        public Func<IEnumerable<IHttpRequestData>, object, Task<IEnumerable<IHttpRequestData>>> HttpRequestDataBinder { get; }
        public Func<IEnumerable<IHttpRequestData>, object> HttpRequestDataExtractor { get; }

        public async Task BindArgumentAsync(IHttpRequest request, object val)
        {
            // check if required - if not and default value - dont bind
            if(!ParameterObject.Required)
            {
                if (val == null)
                {
                    return;
                }
                if (ParameterInfo.ParameterType.IsValueType && val == Activator.CreateInstance(ParameterInfo.ParameterType))
                {
                    return;
                }
            }

            // bind the argument
            await BindPath(typeof(IHttpRequest), request, ArgumentPath.GetEnumerator(), val);
        }

        private async Task<object> BindPath(Type type, object existingValue, IEnumerator<string> pathEnumerator, object value)
        {
            if (typeof(IEnumerable<IHttpRequestData>).IsAssignableFrom(type))
            {
                var lst = ((IEnumerable<IHttpRequestData>)existingValue).ToList();
                var requestData = await HttpRequestDataBinder(lst, value);
                lst.AddRange(requestData.Where(o => !lst.Contains(o)));
                return lst;
            }
            if (typeof(IHttpRequestData).IsAssignableFrom(type))
            {
                return (await HttpRequestDataBinder(SolidHttpRequestData.EmptyArray, value)).Single();
            }

            // if we have reach end of path - return value
            if (!pathEnumerator.MoveNext())
            {
                return value;
            }
            var pathElement = pathEnumerator.Current;
            if (string.IsNullOrEmpty(pathElement))
            {
                return null;
            }
            if (typeof(string).IsAssignableFrom(type))
            {
                var str = (string) existingValue;
                var requestData = await HttpRequestDataBinder(SolidHttpRequestData.EmptyArray, value);
                var strVals = requestData.Select(o => o.GetStringValue());
                str = str.Replace($"{{{pathElement}}}", string.Join("", strVals));
                return str;
            }
            //
            // set value on target.
            //
            var props = existingValue.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            var prop = props.FirstOrDefault(o => o.Name == pathElement);
            if(prop == null)
            {
                throw new Exception($"Cannot find path {pathElement} in {existingValue.GetType().FullName}");
            }
            var propValue = prop.GetValue(existingValue);
            var newValue = await BindPath(prop.PropertyType, propValue, pathEnumerator, value);
            if(!ReferenceEquals(newValue, propValue))
            {
                prop.SetValue(existingValue, newValue);
            }
            return newValue;
        }

        public Task<object> ExtractArgumentAsync(IHttpRequest request)
        {
            return ExtractPath(request, ArgumentPath.GetEnumerator(), false, request);
        }

        private async Task<object> ExtractPath(IHttpRequest request, IEnumerator<string> pathEnumerator, bool filteredList, object val)
        {
            // if we have reach end of path - return value
            if (!pathEnumerator.MoveNext())
            {
                if(val is IEnumerable<IHttpRequestData> rdData2)
                {
                    // add default value if no value supplied and we have one specified
                    if(!rdData2.Any() && DefaultValue != null)
                    {
                        rdData2 = DefaultValue;
                    }
                    return HttpRequestDataExtractor(rdData2);
                }
                return val;
            }
            var pathElement = pathEnumerator.Current;
            if(pathElement == null)
            {
                return null;
            }
            if(pathElement == "")
            {
                return await ExtractRequest(request);
            }
            if(val is IEnumerable<IHttpRequestData> rdEnum)
            {
                if(!filteredList)
                {
                    var subVal = rdEnum.Where(o => o.Name == pathElement);
                    if(!subVal.Any() && pathElement == "body")
                    {
                        return ExtractFormData(rdEnum);
                    }
                    return await ExtractPath(request, pathEnumerator, true, subVal);
                }
                val = rdEnum.FirstOrDefault();
            }
            if (val is string)
            {
                throw new Exception("Cannot extract args from path!");
            }
            //
            // get value from target.
            //
            var props = val.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            var prop = props.FirstOrDefault(o => o.Name == pathElement);
            if (prop != null)
            {
                var propValue = prop.GetValue(val);
                var newValue = await ExtractPath(request, pathEnumerator, filteredList, propValue);
                return newValue;
            }
            if (val is IHttpRequestData rd)
            {
                if(rd.ContentType == "application/json")
                {
                    var newValue = JsonHelper.Deserialize(rd.GetStreamValue(), ParameterInfo.ParameterType, rd.Encoding);
                    return newValue;
                }
                throw new Exception("Cannot extract args from request data!");
            }
            throw new Exception($"Cannot find path {pathElement} in {val.GetType().FullName}");
        }

        private object ExtractFormData(IEnumerable<IHttpRequestData> rdEnum)
        {
            var obj = Activator.CreateInstance(ParameterInfo.ParameterType);
            foreach(var p in ParameterInfo.ParameterType.GetProperties())
            {
                var nullable = p.PropertyType.IsNullableType(out Type npt);
                var arg = rdEnum.Where(o => string.Equals(o.Name, p.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if(arg != null)
                {
                    var val = SolidHttpRequestData.CreateExtractor(arg.ContentType, arg.Name, p.PropertyType, nullable)(arg);
                    p.SetValue(obj, val);
                }
            }
            return obj;
        }

        private async Task<object> ExtractRequest(IHttpRequest request)
        {
            var httpRequest = new HttpRequest();
            await request.CopyToAsync(httpRequest);
            if(ParameterInfo.ParameterType.IsAssignableFrom(typeof(HttpRequest)))
            {
                return httpRequest;
            }

            var template = HttpRequestTemplate.GetTemplate(ParameterInfo.ParameterType);
            return template.CopyToTemplatedInstance(httpRequest);
        }
    }
}