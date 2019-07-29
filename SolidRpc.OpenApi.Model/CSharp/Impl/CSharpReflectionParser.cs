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
                cSharpType = cSharpRepository.GetClass(type.FullName);
            }
            else if (type.IsInterface)
            {
                cSharpType = cSharpRepository.GetInterface(type.FullName);
            }
            else
            {
                cSharpType = cSharpRepository.GetType(type.FullName);
            }
            return cSharpType;
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
