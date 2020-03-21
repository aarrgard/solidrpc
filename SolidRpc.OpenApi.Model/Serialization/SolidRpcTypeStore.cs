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
        private static ConcurrentDictionary<Type, string> s_type2string = new ConcurrentDictionary<Type, string>();
        private static ConcurrentDictionary<string, Type> s_string2type = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Returns the type name for supplied type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(Type type)
        {
            return s_type2string.GetOrAdd(type, GetTypeNameInternal);
       }

        private static string GetTypeNameInternal(Type type)
        {
            if (type.IsGenericTypeDefinition)
            {
                return type.FullName.Substring(0, type.FullName.Length - 2);
            }
            if (type.IsGenericType)
            {
                var genTypeName = GetTypeName(type.GetGenericTypeDefinition());
                var genArgNames = type.GetGenericArguments().Select(o => GetTypeName(o));
                return $"{genTypeName}<{string.Join(",", genArgNames)}>";
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
            throw new Exception("Cannot fínd type:"+typeName);
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