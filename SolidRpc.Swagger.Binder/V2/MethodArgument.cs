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
            return new[] { MapScope(ParameterObject.In), ParameterObject.Name };
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
            BindPath(request, ArgumentPath, val);
        }

        private void BindPath(object target, IEnumerable<string> path, object value)
        {
            var elements = path.ToList();
            if(elements.FirstOrDefault() == null)
            {
                return;
            }
            for(int i = 0; i < elements.Count - 1; i++)
            {
                target = BindPath(target, elements[i]);
            }
            BindValue(target, elements.Last(), value);
        }

        private void BindValue(object target, string pathElement, object value)
        {
            if(target is IDictionary<string, IEnumerable<string>> dict)
            {
                var strVals = Convert(ParameterInfo.ParameterType, ParameterObject, value);
                IEnumerable<string> values;
                if(dict.TryGetValue(pathElement, out values))
                {
                    dict[pathElement] = values.Union(strVals).ToArray();
                }
                else
                {
                    dict[pathElement] = strVals;
                }
                return;
            }
            if (target is string str)
            {
                var strVals = Convert(ParameterInfo.ParameterType, ParameterObject, value);
                str = str.Replace($"{{{pathElement}}}", String.Join("", strVals));
                return;
            }

            //
            // set value on target.
            //
            var props = target.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            var prop = props.First(o => o.Name == pathElement);
            prop.SetValue(target, value);
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