using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.Serialization
{
    /// <summary>
    /// The type store is responsible for mapping clr types on to type names and vice versa.
    /// </summary>
    public class SolidRpcTypeStore
    {
        private static ConcurrentDictionary<string, string> s_type2string = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, Type> s_string2type = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Returns the type name for supplied type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(Type type)
        {
            return GetTypeName(type, false);
        }

        /// <summary>
        /// Returns the type name for supplied type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="treatEnumsAsArrays"></param>
        /// <returns></returns>
        public static string GetTypeName(Type type, bool treatEnumsAsArrays)
        {
            var key = $"{treatEnumsAsArrays}:{type.FullName}";
            return s_type2string.GetOrAdd(key, _ => GetTypeNameInternal(type, treatEnumsAsArrays));
        }

        private static string GetTypeNameInternal(Type type, bool treatEnumsAsArrays)
        {
            if (type.IsGenericTypeDefinition)
            {
                return type.FullName.Substring(0, type.FullName.Length - 2);
            }
            if (type.IsGenericType)
            {
                var genTypeName = GetTypeName(type.GetGenericTypeDefinition());
                var genArgNames = type.GetGenericArguments().Select(o => GetTypeName(o));
                if (genTypeName == "System.Collections.Generic.IEnumerable" && treatEnumsAsArrays)
                {
                    return $"{string.Join(",", genArgNames)}[]";
                }
                else
                {
                    return $"{genTypeName}<{string.Join(",", genArgNames)}>";
                }
            }
            if (type.IsArray)
            {
                return $"{GetTypeName(type.GetElementType())}[]";
            }
            return type.FullName;
        }

        public static Type GetType(string typeName)
        {
            return s_string2type.GetOrAdd(typeName, GetTypeInternal);
        }

        private static Type GetTypeInternal(string typeName)
        {
            if(typeName.EndsWith("[]"))
            {
                return GetTypeInternal(typeName.Substring(0,typeName.Length-2)).MakeArrayType();
            }
            var (genType, genArgs, rest) = ReadType(typeName);
            if (genArgs != null) genType = $"{genType}`{genArgs.Count}";
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var t = a.GetType(genType);
                    if (t != null)
                    {
                        if(genArgs != null)
                        {
                            return t.MakeGenericType(genArgs.ToArray());
                        }
                        return t;
                    }
                }
                catch
                {
                }
            }
            throw new Exception("Cannot find type:"+typeName);
        }

        private static (string, IList<Type>, string) ReadType(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return (fullName, null, null);
            }
            if (fullName.StartsWith(">"))
            {
                return (null, null, fullName.Substring(1));
            }
            var genIdxStart = fullName.IndexOf('<');
            if (genIdxStart == -1)
            {
                var genIdxEnd = fullName.IndexOf('>');
                if (genIdxEnd > -1)
                {
                    return (fullName.Substring(0, genIdxEnd), null, fullName.Substring(genIdxEnd));
                }
                else
                {
                    return (fullName, null, "");
                }
            }

            var genArgs = new List<Type>();
            var genType = fullName.Substring(0, genIdxStart);
            var work = fullName.Substring(genIdxStart + 1);
            var rest = "";
            while (work != null)
            {
                string argType;
                IList<Type> args;
                (argType, args, rest) = ReadType(work);
                if (!string.IsNullOrEmpty(argType))
                {
                    if (args == null)
                    {
                        genArgs.Add(GetType(argType));
                    }
                    else
                    {
                        throw new Exception();
                        //genArgs.Add($"{argType}<{string.Join(",", args)}>");
                    }
                }
                work = rest;
            }
            return (genType, genArgs, rest);
        }
    }
}