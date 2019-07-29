using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// This class constructs a CSharp repository by adding
    /// compiled methods. The repository can then be used to create
    /// an OpenApi spec.
    /// </summary>
    public class CSharpReflectionParser
    {
        /// <summary>
        /// Adds the supplied method to the supplied repository.
        /// </summary>
        /// <param name="cSharpRepository"></param>
        /// <param name="method"></param>
        public static void AddMethod(ICSharpRepository cSharpRepository, MethodInfo method)
        {
            var returnType = GetType(cSharpRepository, method.ReturnType);
            var cSharpType = GetType(cSharpRepository, method.DeclaringType);
            var cSharpMethod = new CSharpMethod(cSharpType, method.Name, returnType);
            method.GetParameters().ToList().ForEach(o =>
            {
                var parameterType = GetType(cSharpRepository, o.ParameterType);
                var cSharpMethodParameter = new CSharpMethodParameter(cSharpMethod, o.Name, parameterType, o.IsOptional);
                cSharpMethod.AddMember(cSharpMethodParameter);
            });

            cSharpType.AddMember(cSharpMethod);
        }

        private static ICSharpType GetType(ICSharpRepository cSharpRepository, Type type)
        {
            if(type.IsTaskType(out Type taskType))
            {
                type = taskType;
            }
            ICSharpType cSharpType;
            if (type.IsClass)
            {
                cSharpType = cSharpRepository.GetClass(CreateTypeName(type));
                if(!cSharpType.Properties.Any())
                {
                    type.GetProperties().ToList().ForEach(o =>
                    {
                        var propertyType = GetType(cSharpRepository, o.PropertyType);
                        var cSharpProperty = new CSharpProperty(cSharpType, o.Name, propertyType);
                        cSharpType.AddMember(cSharpProperty);
                    });
                }
            }
            else if (type.IsInterface)
            {
                cSharpType = cSharpRepository.GetInterface(CreateTypeName(type));
            }
            else
            {
                cSharpType = cSharpRepository.GetType(CreateTypeName(type));
            }
            if (type.IsGenericType)
            {
                type.GetGenericArguments().ToList().ForEach(o => GetType(cSharpRepository, o));
            }
            return cSharpType;
        }

        private static string CreateTypeName(Type type)
        {
            if(type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                var genTypeName = genType.FullName;
                genTypeName = genTypeName.Substring(0, genTypeName.IndexOf('`'));
                return $"{genTypeName}<{string.Join(",",type.GetGenericArguments().Select(o => CreateTypeName(o)))}>";
            }
            return type.FullName;
        }

        /// <summary>
        /// Adds all the type methods to the repository.
        /// </summary>
        /// <param name="cSharpRepository"></param>
        /// <param name="type"></param>
        public static void AddType(CSharpRepository cSharpRepository, Type type)
        {
            type.GetMethods().ToList().ForEach(o => AddMethod(cSharpRepository, o));
        }
    }
}
