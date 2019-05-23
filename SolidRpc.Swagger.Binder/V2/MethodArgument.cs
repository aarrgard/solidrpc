using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SolidRpc.Swagger.Model.V2;

namespace SolidRpc.Swagger.Binder.V2
{
    public class MethodArgument : IMethodArgument
    {
        public MethodArgument(ParameterObject parameterObject, ParameterInfo parameterInfo)
        {
            ParameterObject = parameterObject ?? throw new ArgumentNullException(nameof(parameterObject));
            ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterObject));
            ArgumentPath = CreatePath();

            if(parameterObject.Name != parameterInfo.Name)
            {
                throw new Exception("Name mismatch");
            }
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

        public void BindArgument(IHttpRequest request, object val)
        {
            BindPath(request, ArgumentPath.GetEnumerator(), val);
        }

        private object BindPath(object target, IEnumerator<string> enumerator, object value)
        {
            // if we have reach end of path - return value
            if(!enumerator.MoveNext())
            {
                return value;
            }
            var pathElement = enumerator.Current;
            if(string.IsNullOrEmpty(pathElement))
            {
                return null;
            }
            if (target is IEnumerable<HttpRequestData> data)
            {
                var lst = data.ToList();
                var oldReqData = lst.FirstOrDefault(o => o.Name == pathElement);
                value = BindPath(oldReqData, enumerator, value);
                if(oldReqData == null)
                {
                    lst.Add(HttpRequestData.Create(pathElement, value));
                }
                else
                {
                    var idx = lst.IndexOf(oldReqData);
                    lst[idx] = oldReqData + HttpRequestData.Create(pathElement, value);
                }
                return lst;
            }
            if (target is string str)
            {
                var strVals = Convert(ParameterInfo.ParameterType, ParameterObject, value);
                str = str.Replace($"{{{pathElement}}}", string.Join("", strVals));
                return str;
            }
            //
            // set value on target.
            //
            var props = target.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            var prop = props.FirstOrDefault(o => o.Name == pathElement);
            if(prop == null)
            {
                throw new Exception($"Cannot find path {pathElement} in {target.GetType().FullName}");
            }
            var propValue = prop.GetValue(target);
            if (propValue == null)
            {
                propValue = Activator.CreateInstance(prop.PropertyType);
            }
            var newValue = BindPath(propValue, enumerator, value);
            if(!ReferenceEquals(newValue, propValue))
            {
                prop.SetValue(target, newValue);
            }
            return newValue;
        }

        private IEnumerable<string> Convert(Type type, ItemBase schema, object value)
        {
            switch(schema.Type)
            {
                case "array":
                    return CreateArray(type, schema, value);
                case "string":
                    return CreateString(type, schema, value);
                case "integer":
                    return new[] { System.Convert.ToString(value) };
                default:
                    throw new Exception("Cannot handle schema object:"+ schema.Type);
            }
        }

        private IEnumerable<string> CreateString(Type type, ItemBase schema, object value)
        {
            IEnumerable<string> strings;
            if(value is string str)
            {
                strings = new string[] { str };
            }
            else
            {
                throw new Exception("Cannot handle value:" + type.FullName);
            }

            if(schema.Enum != null)
            {
                var errorString = strings.FirstOrDefault(o => !schema.Enum.Contains(o));
                if (errorString != null)
                {
                    throw new FormatException($"String({errorString}) does not match({string.Join(",", schema.Enum)})");
                }
            }
            return strings;
        }

        private IEnumerable<string> CreateArray(Type type, ItemBase schema, object value)
        {
            if (!type.IsGenericType) throw new ArgumentException("Type is not generic");
            if (!typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition())) throw new ArgumentException("Type is not enumerable");
            var valueType = type.GetGenericArguments()[0];
            var m = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(o => o.Name == nameof(CreateArrayGeneric))
                .Single();
            return (IEnumerable<string>)m.MakeGenericMethod(valueType).Invoke(this, new[] { schema, value});
        }
        private IEnumerable<string> CreateArrayGeneric<T>(ItemBase schema, object value)
        {
            var strings = ((IEnumerable<T>)value).SelectMany(o => Convert(typeof(T), schema.Items, o)).ToList();
            switch (schema.CollectionFormat)
            {
                case null:
                case "csv":
                    return new string[] { string.Join(",", strings) };
                case "ssv":
                    return new string[] { string.Join(" ", strings) };
                case "tsv":
                    return new string[] { string.Join("\t", strings) };
                case "pipes":
                    return new string[] { string.Join("|", strings) };
                case "multi":
                    return strings;
                default:
                    throw new Exception("Cannot handle collection format:" + schema.CollectionFormat);
            }
        }

        private object BindPath(object target, string pathElement)
        {
            var props = target.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            var prop = props.First(o => o.Name == pathElement);
            return prop.GetValue(target);
        }
    }
}