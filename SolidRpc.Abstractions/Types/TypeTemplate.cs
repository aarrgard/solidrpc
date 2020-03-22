using Microsoft.Extensions.Primitives;
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
                        break;
                    }
                }

                var getMethod = GetType().GetProperty("Get" + prop.Name);
                if(getMethod == null)
                {
                    throw new Exception("No Get method defined for template property:"+ prop.Name);
                }
                var getterFunc = CreateGetter(getMethod.PropertyType, prop.PropertyType, otherProp);
                if(getterFunc != null)
                {
                    getMethod.SetValue(this, getterFunc);
                }
                else
                {
                    isTemplateType = false;
                    break;
                }


                var setMethod = GetType().GetProperty("Set" + prop.Name);
                if (setMethod == null)
                {
                    throw new Exception("No Set method defined for template property:" + prop.Name);
                }
                var setterFunc = CreateSetter(setMethod.PropertyType, prop.PropertyType, otherProp);
                if(setterFunc != null)
                {
                    setMethod.SetValue(this, setterFunc);
                }
                else
                {
                    isTemplateType = false;
                    break;
                }
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

        private object CreateSetterInternal<T>(Type actionType, PropertyInfo pi)
        {
            if (pi == null)
            {
                return (Action<object, T>)((_, __) => { });
            }
            if (typeof(T) != pi.PropertyType)
            {
                return null;
            }
            if (actionType != typeof(Action<object, T>))
            {
                throw new Exception($"Property type does not match getter function for property {pi.Name}.");
            }
            return (Action<object, T>)((_, __) => pi.SetValue(_, __));
        }
        private object CreateSetter(Type actionType, Type propertyType, PropertyInfo pi)
        {
            var methods = typeof(TypeTemplate).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).AsEnumerable();
            methods = methods.Where(o => o.IsGenericMethod);
            methods = methods.Where(o => o.Name == nameof(CreateSetterInternal));
            return methods.Single().MakeGenericMethod(propertyType).Invoke(this, new object[] { actionType, pi });
        }

        private object CreateGetterInternal<T>(Type functionType, PropertyInfo pi)
        {
            if (pi == null)
            {
                return (Func<object, T>)(_ => default(T));
            }
            if (typeof(T) != pi.PropertyType)
            {
                return null;
            }
            if (functionType != typeof(Func<object, T>))
            {
                throw new Exception($"Property type does not match getter function for property {pi.Name}.");
            }
            return (Func<object, T>)((_) => (T)pi.GetValue(_));
        }

        private object CreateGetter(Type functionType, Type propertyType, PropertyInfo pi)
        {
            var methods = typeof(TypeTemplate).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).AsEnumerable();
            methods = methods.Where(o => o.IsGenericMethod);
            methods = methods.Where(o => o.Name == nameof(CreateGetterInternal));
            return methods.Single().MakeGenericMethod(propertyType).Invoke(this, new object[] { functionType, pi });
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
