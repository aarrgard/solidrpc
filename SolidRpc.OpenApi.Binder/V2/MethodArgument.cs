﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SolidRpc.OpenApi.Model.V2;

namespace SolidRpc.OpenApi.Binder.V2
{
    public class MethodArgument : IMethodArgument
    {
        public MethodArgument(ParameterObject parameterObject, ParameterInfo parameterInfo)
        {
            ParameterObject = parameterObject ?? throw new ArgumentNullException(nameof(parameterObject));
            ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterObject));
            ArgumentPath = CreatePath();
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

            if (ParameterObject.IsFileType())
            {
                HttpRequestDataBinder = (_, __) => SetFileData(ParameterObject.Name, _, __);
            }
            else
            {
                HttpRequestDataBinder = HttpRequestData.CreateBinder(contentType, parameterObject.Name, ParameterInfo.ParameterType, collectionFormat);
            }
        }

        private IEnumerable<HttpRequestData> SetFileData(string name, IEnumerable<HttpRequestData> formData, object value)
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
                    latest.SetBinaryData(name, (Stream)value);
                    return new[] { latest };
            }
        }

        private HttpRequestDataBinary GetLatestBinaryData(IEnumerable<HttpRequestData> formData)
        {
            var latest = formData.OfType<HttpRequestDataBinary>().LastOrDefault();
            if(latest == null)
            {
                latest = new HttpRequestDataBinary("application/octet-stream", "temp", null);
            }
            return latest;
        }

        private IEnumerable<string> CreatePath()
        {
            var scope = MapScope(ParameterObject.In);
            if(scope == nameof(IHttpRequest.Body))
            {
                return new[] { scope };
            }
            else
            {
                return new[] { scope, ParameterObject.Name };
            }
        }

        private string MapScope(string scope)
        {
            switch(scope)
            {
                case "skip":
                    return null;
                case "query":
                    return nameof(IHttpRequest.Query);
                case "path":
                    return nameof(IHttpRequest.Path);
                case "formData":
                    return nameof(IHttpRequest.FormData);
                case "body":
                    return nameof(IHttpRequest.Body);
                default:
                    throw new Exception("Cannot handle scope:" + scope);
            }
        }

        public ParameterObject ParameterObject { get; }

        public ParameterInfo ParameterInfo { get; }

        public string Name => ParameterObject.Name;

        public IEnumerable<string> ArgumentPath { get; }

        public Func<IEnumerable<HttpRequestData>, object, IEnumerable<HttpRequestData>> HttpRequestDataBinder { get; }

        public void BindArgument(IHttpRequest request, object val)
        {
            BindPath(typeof(IHttpRequest), request, ArgumentPath.GetEnumerator(), val);
        }

        private object BindPath(Type type, object existingValue, IEnumerator<string> pathEnumerator, object value)
        {
            if (typeof(IEnumerable<HttpRequestData>).IsAssignableFrom(type))
            {
                var lst = ((IEnumerable<HttpRequestData>)existingValue).ToList();
                var requestData = HttpRequestDataBinder(lst, value);
                lst.AddRange(requestData.Where(o => !lst.Contains(o)));
                return lst;
            }
            if (typeof(HttpRequestData).IsAssignableFrom(type))
            {
                return HttpRequestDataBinder(HttpRequestData.EmptyArray, value).Single();
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
                var requestData = HttpRequestDataBinder(HttpRequestData.EmptyArray, value);
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
    }
}