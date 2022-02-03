using SolidRpc.OpenApi.Model.CodeDoc;
using SolidRpc.OpenApi.Model.CodeDoc.Impl;
using System;
using System.Collections.Generic;
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
        private static ICodeDocRepository s_codeDocRepository = new CodeDocRepository();

        /// <summary>
        /// Adds the supplied method to the supplied repository.
        /// </summary>
        /// <param name="cSharpRepository"></param>
        /// <param name="method"></param>
        public static ICSharpMethod AddMethod(ICSharpRepository cSharpRepository, MethodInfo method)
        {
            var returnType = GetType(cSharpRepository, method.ReturnType);
            var cSharpType = GetType(cSharpRepository, method.DeclaringType);
            var cSharpMethod = new CSharpMethod(cSharpType, method.Name, returnType);
            cSharpMethod.ParseComment(s_codeDocRepository.GetMethodDoc(method).CodeComments);
            method.GetParameters().ToList().ForEach(o =>
            {
                var parameterType = GetType(cSharpRepository, o.ParameterType);
                var cSharpMethodParameter = new CSharpMethodParameter(cSharpMethod, o.Name, parameterType, IsOptional(o));
                AddAttributes(cSharpMethodParameter, o.CustomAttributes);
                cSharpMethod.AddMember(cSharpMethodParameter);
            });

            AddAttributes(cSharpMethod, method.CustomAttributes);
            cSharpType.AddMember(cSharpMethod);
            return cSharpMethod;
        }

        private static bool IsOptional(ParameterInfo o)
        {
            if(o.IsOptional)
            {
                return true;
            }
            if(o.ParameterType.IsNullableType(out Type nullableType))
            {
                return true;
            }
            return false;
        }

        private static ICSharpType GetType(ICSharpRepository cSharpRepository, Type type)
        {
            if(type.IsTaskType(out Type taskType))
            {
                type = taskType ?? type; // null means just Task not Task<T>
            }
            ICSharpType cSharpType;
            if (type.IsClass)
            {
                cSharpType = cSharpRepository.GetClass(CreateTypeName(type));
                if (cSharpType.Comment == null)
                {
                    AddAttributes(cSharpType, type.CustomAttributes);
                    cSharpType.ParseComment(s_codeDocRepository.GetClassDoc(type)?.CodeComments);
                    type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).ToList().ForEach(o =>
                    {
                        var propertyType = GetType(cSharpRepository, o.PropertyType);
                        var cSharpProperty = new CSharpProperty(cSharpType, o.Name, propertyType);
                        cSharpType.AddMember(cSharpProperty);
                    });
                    if (type.BaseType != null && type.BaseType != typeof(object))
                    {
                        cSharpType.AddExtends(GetType(cSharpRepository, type.BaseType));
                    }
                }
            }
            else if (type.IsInterface)
            {
                cSharpType = cSharpRepository.GetInterface(CreateTypeName(type));
                if (cSharpType.Comment == null)
                {
                    AddAttributes(cSharpType, type.CustomAttributes);
                    cSharpType.ParseComment(s_codeDocRepository.GetClassDoc(type)?.CodeComments);
                }
            }
            else
            {
                cSharpType = cSharpRepository.GetType(CreateTypeName(type));
                if(cSharpType == null)
                {
                    throw new Exception("Cannot find type:"+ CreateTypeName(type));
                }
                if (cSharpType.Comment == null)
                {
                    AddAttributes(cSharpType, type.CustomAttributes);
                    cSharpType.ParseComment(s_codeDocRepository.GetClassDoc(type)?.CodeComments);
                }
            }
            if (type.IsGenericType)
            {
                type.GetGenericArguments().ToList().ForEach(o => GetType(cSharpRepository, o));
            }
            return cSharpType;
        }

        private static void AddAttributes(ICSharpMember cSharpType, IEnumerable<CustomAttributeData> attributes)
        {
            foreach(var a in attributes)
            {
                var ags = a.ConstructorArguments.Select(o => o.Value).ToList();
                var namedArgs = a.NamedArguments.ToDictionary(o => o.MemberName, o => o.TypedValue.Value);
                cSharpType.AddMember(new CSharpAttribute(cSharpType, a.AttributeType.FullName, ags, namedArgs));
            }
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
