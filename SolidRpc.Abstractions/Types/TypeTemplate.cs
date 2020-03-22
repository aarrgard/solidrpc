using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SolidRpc.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeTemplate
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="templateType"></param>
        /// <param name="otherType"></param>
        /// <param name="requiredProps"></param>
        public TypeTemplate(Type templateType, Type otherType, IEnumerable<string> requiredProps)
        {
            requiredProps = requiredProps.Select(o => o.ToLower()).ToList();
            var otherProperties = otherType.GetProperties().ToDictionary(o => o.Name.ToLower(), o => o);

            bool isTemplateType = true;
            foreach(var prop in templateType.GetProperties())
            {
                var propName = prop.Name.ToLower();
                if (!otherProperties.TryGetValue(propName, out PropertyInfo otherProp))
                {
                    if(requiredProps.Contains(propName))
                    {
                        isTemplateType = false;
                    }
                }

                var getMethod = GetType().GetProperty("Get" + prop.Name);
                if(getMethod == null)
                {
                    throw new Exception("No Get method defined for template property:"+ prop.Name);
                }
                getMethod.SetValue(this, CreateGetter(getMethod.PropertyType, otherProp));
                var setMethod = GetType().GetProperty("Set" + prop.Name);
                if (setMethod == null)
                {
                    throw new Exception("No Set method defined for template property:" + prop.Name);
                }
                setMethod.SetValue(this, CreateSetter(setMethod.PropertyType, otherProp));
                otherProperties.Remove(propName);
            }

            IsTemplateType = isTemplateType && otherProperties.Count == 0;
            IsTemplateType = HandleTemplateType(templateType, otherType, IsTemplateType);
        }

        /// <summary>
        /// Adds support for subclasses to override logic.
        /// </summary>
        /// <param name="templateType"></param>
        /// <param name="otherType"></param>
        /// <param name="isTemplateType"></param>
        /// <returns></returns>
        protected virtual bool HandleTemplateType(Type templateType, Type otherType, bool isTemplateType)
        {
            return isTemplateType;
        }

        private object CreateSetter(Type propertyType, PropertyInfo pi)
        {
            if (typeof(Action<object, Stream>).IsAssignableFrom(propertyType))
            {
                if(pi == null)
                {
                    return (Action<object, Stream>)((_, __) => { });
                }
                if (pi.PropertyType != typeof(Stream))
                {
                    throw new Exception($"Property type does not match getter function for property {pi.Name}.");
                }
                return (Action<object, Stream>)((_, __) => pi.SetValue(_, __));
            }
            if (typeof(Action<object, string>).IsAssignableFrom(propertyType))
            {
                if (pi == null)
                {
                    return (Action<object, string>)((_, __) => { });
                }
                if (pi.PropertyType != typeof(string))
                {
                    throw new Exception($"Property type does not match getter function for property {pi.Name}.");
                }
                return (Action<object, string>)((_, __) => pi.SetValue(_, __));
            }
            if (typeof(Action<object, DateTimeOffset?>).IsAssignableFrom(propertyType))
            {
                if (pi == null)
                {
                    return (Action<object, DateTimeOffset?>)((_, __) => { });
                }
                if (pi.PropertyType != typeof(DateTimeOffset?))
                {
                    throw new Exception($"Property type does not match getter function for property {pi.Name}.");
                }
                return (Action<object, DateTimeOffset?>)((_, __) => pi.SetValue(_, __));
            }
            throw new NotImplementedException();
        }

        private object CreateGetter(Type propertyType, PropertyInfo pi)
        {
            if (typeof(Func<object, Stream>).IsAssignableFrom(propertyType))
            {
                if (pi == null)
                {
                    return (Func<object, Stream>)((_) => null);
                }
                if (pi.PropertyType != typeof(Stream))
                {
                    throw new Exception($"Property type does not match getter function for property {pi.Name}.");
                }
                return (Func<object, Stream>)(_ => (Stream)pi.GetValue(_));
            }
            if (typeof(Func<object, string>).IsAssignableFrom(propertyType))
            {
                if (pi == null)
                {
                    return (Func<object, string>)((_) => null);
                }
                if (pi.PropertyType != typeof(string))
                {
                    throw new Exception($"Property type does not match getter function for property {pi.Name}.");
                }
                return (Func<object, string>)(_ => (string)pi.GetValue(_));
            }
            if (typeof(Func<object, DateTimeOffset?>).IsAssignableFrom(propertyType))
            {
                if (pi == null)
                {
                    return (Func<object, DateTimeOffset?>)((_) => null);
                }
                if (pi.PropertyType != typeof(DateTimeOffset?))
                {
                    throw new Exception($"Property type does not match getter function for property {pi.Name}.");
                }
                return (Func<object, DateTimeOffset?>)(_ => (DateTimeOffset?)pi.GetValue(_));
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if this is a template type.
        /// </summary>
        public bool IsTemplateType { get; }
    }

    /// <summary>
    /// The type template can be used to access similar types.
    /// </summary>
    /// <typeparam name="TTemplate"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class TypeTemplate<TTemplate> : TypeTemplate where TTemplate : class
    {
        private static ConcurrentDictionary<Type, TypeTemplate<TTemplate>> s_typeInfo = new ConcurrentDictionary<Type, TypeTemplate<TTemplate>>();

        public static TypeTemplate<TTemplate> GetTemplate(Type type, Func<Type, TypeTemplate<TTemplate>> ctr)
        {
            return s_typeInfo.GetOrAdd(type, ctr);
        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public TypeTemplate(Type otherType, IEnumerable<string> requiredProps)
            :base(typeof(TTemplate), otherType, requiredProps)
        {
        }
    }
}
