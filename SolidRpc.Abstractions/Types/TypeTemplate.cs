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
        /// <param name="templatedType"></param>
        /// <param name="requiredProps"></param>
        public TypeTemplate(Type templateType, Type templatedType, IEnumerable<string> requiredProps)
        {
            TemplateType = templateType;
            TemplatedType = templatedType;

            Templated2TemplateType = new List<Action<object, object>>();
            Template2TemplatedType = new List<Action<object, object>>();
            requiredProps = requiredProps.Select(o => o.ToLower()).ToList();
            var otherProperties = templatedType.GetProperties().ToDictionary(o => o.Name.ToLower(), o => o);

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
                if(getMethod != null)
                {
                    var getter = CreateGetter(prop.PropertyType, otherProp);
                    getMethod.SetValue(this, getter);
                }


                var setMethod = GetType().GetProperty("Set" + prop.Name);
                if (setMethod != null)
                {
                    var setter = CreateSetter(prop.PropertyType, otherProp);
                    setMethod.SetValue(this, setter);
                }

                otherProperties.Remove(propName);

                Templated2TemplateType.Add(CreateCopyFunc(templateType, templatedType, prop.PropertyType, prop.Name));
                Template2TemplatedType.Add(CreateCopyFunc(templatedType, templateType, prop.PropertyType, prop.Name));
            }

            IsTemplateType = isTemplateType && otherProperties.Count == 0;
            IsTemplateType = HandleTemplateType(templateType, templatedType, IsTemplateType);
        }

        private Action<object, object> CreateCopyFunc(Type dstType, Type srcType, Type propertyType, string propertyName)
        {
            var methods = typeof(TypeTemplate).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).AsEnumerable();
            methods = methods.Where(o => o.IsGenericMethod);
            methods = methods.Where(o => o.Name == nameof(CreateCopyFunc));
            return (Action<object, object>)methods.Single().MakeGenericMethod(dstType, srcType, propertyType).Invoke(this, new object[] { propertyName });
        }

        private Action<object, object> CreateCopyFunc<TDst, TSrc, TProp>(string propertyName)
        {
            var getter = (Func<object, TProp>)CreateGetterInternal<TProp>(typeof(TSrc).GetProperty(propertyName));
            var setter = (Action<object, TProp>)CreateSetterInternal<TProp>(typeof(TDst).GetProperty(propertyName));
            return (target, src) => setter(target, getter(src));
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

        private object CreateSetterInternal<T>(PropertyInfo pi)
        {
            if (pi == null)
            {
                return (Action<object, T>)((_, __) => { });
            }
            if (typeof(T) != pi.PropertyType)
            {
                return (Action<object, T>)((_, __) => { });
            }
            return (Action<object, T>)((_, __) => {
                pi.SetValue(_, __);
            });
        }
        private object CreateSetter(Type propertyType, PropertyInfo pi)
        {
            var methods = typeof(TypeTemplate).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).AsEnumerable();
            methods = methods.Where(o => o.IsGenericMethod);
            methods = methods.Where(o => o.Name == nameof(CreateSetterInternal));
            return methods.Single().MakeGenericMethod(propertyType).Invoke(this, new object[] { pi });
        }

        private object CreateGetterInternal<T>(PropertyInfo pi)
        {
            if (pi == null)
            {
                return (Func<object, T>)(_ => default(T));
            }
            if (typeof(T) != pi.PropertyType)
            {
                return (Func<object, T>)(_ => default(T));
            }
            return (Func<object, T>)((_) => {
                return (T)pi.GetValue(_);
            });
        }

        private object CreateGetter(Type propertyType, PropertyInfo pi)
        {
            var methods = typeof(TypeTemplate).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).AsEnumerable();
            methods = methods.Where(o => o.IsGenericMethod);
            methods = methods.Where(o => o.Name == nameof(CreateGetterInternal));
            return methods.Single().MakeGenericMethod(propertyType).Invoke(this, new object[] { pi });
        }

        /// <summary>
        /// Returns true if this is a template type.
        /// </summary>
        public bool IsTemplateType { get; }

        /// <summary>
        /// The templated type
        /// </summary>
        public Type TemplateType { get; }

        /// <summary>
        /// The templated type
        /// </summary>
        public Type TemplatedType { get; }

        /// <summary>
        /// Sets the properties on one object
        /// </summary>
        protected ICollection<Action<object, object>> Templated2TemplateType { get; }

        /// <summary>
        /// Sets the properties on another object
        /// </summary>
        protected ICollection<Action<object, object>> Template2TemplatedType { get; }
    }

    /// <summary>
    /// The type template can be used to access similar types.
    /// </summary>
    /// <typeparam name="TTemplate"></typeparam>
    public class TypeTemplate<TTemplate> : TypeTemplate where TTemplate : class
    {
        private static ConcurrentDictionary<Type, TypeTemplate<TTemplate>> s_typeInfo = new ConcurrentDictionary<Type, TypeTemplate<TTemplate>>();

        /// <summary>
        /// Returns the template for supplied type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ctr"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Copies the information in a template instance to a templated instance.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object CopyToTemplatedInstance(TTemplate source)
        {
            var x = Activator.CreateInstance(TemplatedType);
            Template2TemplatedType.ToList().ForEach(o => o(x, source));
            return x;
        }
    }
}
