using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
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
            if (parameterObject.Name != parameterInfo.Name)
            {
                throw new Exception("Name mismatch");
            }

            var contentType = "text/plain";
            if(ParameterObject.In == "body")
            {
                contentType = ParameterObject.GetParent<OperationObject>().GetConsumes().FirstOrDefault();
            }

            var collectionFormat = ParameterObject.Type == "array" ? ParameterObject.CollectionFormat ?? "csv" : null;

            if (ParameterObject.IsBinaryType())
            {
                HttpRequestDataBinder = (_, __) => SetFileData(ParameterObject.Name, _, __);
                HttpRequestDataExtractor = GetFileData(contentType, ParameterObject.Name, ParameterInfo.ParameterType);
            }
            else
            {
                HttpRequestDataBinder = SolidHttpRequestData.CreateBinder(contentType, parameterObject.Name, ParameterInfo.ParameterType, collectionFormat);
                HttpRequestDataExtractor = SolidHttpRequestData.CreateExtractor(contentType, parameterObject.Name, ParameterInfo.ParameterType, collectionFormat);
            }
        }

        private Func<IEnumerable<IHttpRequestData>, object> GetFileData(string contentType, string name, Type type)
        {
            if(type.IsFileType())
            {
                return (_) =>
                {
                    var valData = _.FirstOrDefault();
                    var data = Activator.CreateInstance(type);
                    type.SetFileTypeStreamData(data, valData.GetBinaryValue());
                    type.SetFileTypeContentType(data, valData.ContentType);
                    type.SetFileTypeFilename(data, valData.Filename);
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
            return SolidHttpRequestData.CreateExtractor(contentType, name, type, null);
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
                return new[] { MapScope(ParameterObject.In), "body" };
            }
            if (ParameterInfo.ParameterType == typeof(CancellationToken))
            {
                return new[] { "CancellationToken" };
            }
            return new[] { MapScope(ParameterObject.In), ParameterObject.Name };
        }

        private IEnumerable<IHttpRequestData> SetFileData(string name, IEnumerable<IHttpRequestData> formData, object value)
        {
            var latest = GetLatestBinaryData(formData);
            switch (name.ToLower())
            {
                case "contenttype":
                    latest.SetContentType((string)value);
                    return new[] { latest };
                case "filename":
                    latest.SetFilename((string)value);
                    return new[] { latest };
                default:
                    if(value is Stream stream)
                    {
                        latest.SetBinaryData(name, stream);
                    }
                    else if(value is byte[] bytes)
                    {
                        latest.SetBinaryData(name, new MemoryStream(bytes));
                    }
                    else
                    {
                        latest.SetBinaryData(name, TypeExtensions.GetFileTypeStreamData(value.GetType(), value));
                        latest.SetContentType(TypeExtensions.GetFileTypeContentType(value.GetType(), value));
                        latest.SetFilename(TypeExtensions.GetFileTypeFilename(value.GetType(), value));
                    }
                    return new[] { latest };
            }
        }

        private SolidHttpRequestDataBinary GetLatestBinaryData(IEnumerable<IHttpRequestData> formData)
        {
            var latest = formData.OfType<SolidHttpRequestDataBinary>().LastOrDefault();
            if(latest == null)
            {
                latest = new SolidHttpRequestDataBinary("application/octet-stream", "temp", (byte[])null);
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

        public ParameterObject ParameterObject { get; }

        public ParameterInfo ParameterInfo { get; }

        public string Name => ParameterObject.Name;

        public IEnumerable<string> ArgumentPath { get; }

        public Func<IEnumerable<IHttpRequestData>, object, IEnumerable<IHttpRequestData>> HttpRequestDataBinder { get; }
        public Func<IEnumerable<IHttpRequestData>, object> HttpRequestDataExtractor { get; }

        public Task BindArgumentAsync(IHttpRequest request, object val)
        {
            // check if required - if not and default value - dont bind
            if(!ParameterObject.Required)
            {
                if (val == null)
                {
                    return Task.CompletedTask;
                }
                if (ParameterInfo.ParameterType.IsValueType && val == Activator.CreateInstance(ParameterInfo.ParameterType))
                {
                    return Task.CompletedTask;
                }
            }

            // bind the argument
            BindPath(typeof(IHttpRequest), request, ArgumentPath.GetEnumerator(), val);
            return Task.CompletedTask;
        }

        private object BindPath(Type type, object existingValue, IEnumerator<string> pathEnumerator, object value)
        {
            if (typeof(IEnumerable<IHttpRequestData>).IsAssignableFrom(type))
            {
                var lst = ((IEnumerable<IHttpRequestData>)existingValue).ToList();
                var requestData = HttpRequestDataBinder(lst, value);
                lst.AddRange(requestData.Where(o => !lst.Contains(o)));
                return lst;
            }
            if (typeof(IHttpRequestData).IsAssignableFrom(type))
            {
                return HttpRequestDataBinder(SolidHttpRequestData.EmptyArray, value).Single();
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
                var requestData = HttpRequestDataBinder(SolidHttpRequestData.EmptyArray, value);
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
            var newValue = BindPath(prop.PropertyType, propValue, pathEnumerator, value);
            if(!ReferenceEquals(newValue, propValue))
            {
                prop.SetValue(existingValue, newValue);
            }
            return newValue;
        }

        public Task<object> ExtractArgumentAsync(IHttpRequest request)
        {
            var res = ExtractPath(request, ArgumentPath.GetEnumerator(), false, request);
            return Task.FromResult(res);
        }

        private object ExtractPath(IHttpRequest request, IEnumerator<string> pathEnumerator, bool filteredList, object val)
        {
            // if we have reach end of path - return value
            if (!pathEnumerator.MoveNext())
            {
                if(val is IEnumerable<IHttpRequestData> rdData2)
                {
                    return HttpRequestDataExtractor(rdData2);
                }
                return val;
            }
            var pathElement = pathEnumerator.Current;
            if (string.IsNullOrEmpty(pathElement))
            {
                return null;
            }
            if(val is IEnumerable<IHttpRequestData> rdEnum)
            {
                if(!filteredList)
                {
                    var subVal = rdEnum.Where(o => o.Name == pathElement);
                    return ExtractPath(request, pathEnumerator, true, subVal);
                }
                val = rdEnum.FirstOrDefault();
            }
            if(val is string)
            {
                throw new Exception("Cannot extract args from path!");
            }
            //
            // get value from target.
            //
            var props = val.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            var prop = props.FirstOrDefault(o => o.Name == pathElement);
            if (prop == null)
            {
                throw new Exception($"Cannot find path {pathElement} in {val.GetType().FullName}");
            }
            var propValue = prop.GetValue(val);
            var newValue = ExtractPath(request, pathEnumerator, filteredList, propValue);
            return newValue;
        }
    }
}