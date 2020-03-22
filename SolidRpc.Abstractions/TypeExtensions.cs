using SolidRpc.Abstractions.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Contains extension methods for the Type
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns true if the type is a binary template type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFileContent(this Type type)
        {
            return FileContentTemplate.GetTemplate(type).IsTemplateType;
        }
        /// <summary>
        /// Returns true if the type is a HttpRequest template type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsHttpRequest(this Type type)
        {
            return HttpRequestTemplate.GetTemplate(type).IsTemplateType;
        }

        /// <summary>
        /// Returns true if the type is an enum
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static bool IsEnumType(this Type type, out Type enumType)
        {
            if (!type.IsGenericType)
            {
                enumType = null;
                return false;
            }
            IEnumerable<Type> interfaceProspects = type.GetInterfaces();
            if(type.IsInterface)
            {
                interfaceProspects = interfaceProspects.Union(new[] { type });
            }
            var enumInterface = interfaceProspects
                .Where(o => o.IsGenericType)
                .Where(o => o.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .FirstOrDefault();
            if (enumInterface == null)
            {
                enumType = null;
                return false;
            }
            enumType = enumInterface.GetGenericArguments()[0];
            return true;
        }

        /// <summary>
        /// Returns true if supplied tyoe is a task type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public static bool IsTaskType(this Type type, out Type taskType)
        {
            if (!type.IsGenericType)
            {
                taskType = null;
                return type == typeof(Task);
            }
            if (!typeof(Task<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                taskType = null;
                return false;
            }
            taskType = type.GetGenericArguments()[0];
            return true;
        }

        /// <summary>
        /// Returns true if supplied type is a nullable type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nullableType"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type, out Type nullableType)
        {
            if (!type.IsGenericType)
            {
                nullableType = null;
                return false;
            }
            if (!typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                nullableType = null;
                return false;
            }
            nullableType = type.GetGenericArguments()[0];
            return true;
        }
    }
}
