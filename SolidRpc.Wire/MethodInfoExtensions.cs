using SolidRpc.Wire;
using System;
using System.Linq;
using System.Reflection;

namespace System.Reflection
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Returns the solid rpc method info structure for supplied method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static SolidRpcMethodInfo GetSolidRpcMethodInfo(this MethodInfo methodInfo)
        {
            var declaringType = methodInfo.DeclaringType;
            var assemblyName = declaringType.Assembly.GetName();
            return new SolidRpcMethodInfo()
            {
                ApiName = assemblyName.Name,
                ApiVersion = assemblyName.Version.ToString(),
                InterfaceName = declaringType.FullName,
                MethodName = methodInfo.Name,
                Arguments = methodInfo.GetParameters().Select(o => new SolidRpcMethodArg()
                {
                    Name = o.Name,
                    ValueType = SolidRpcTypeStore.GetTypeName(o.ParameterType)
                }).ToArray(),
                ReturnType = SolidRpcTypeStore.GetTypeName(methodInfo.ReturnType)
            };
        }
    }
}



namespace SolidRpc.Wire
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Returns the solid rpc method info structure for supplied method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(this SolidRpcMethodInfo methodInfo)
        {
            foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(a.GetName().Name != methodInfo.ApiName)
                {
                    continue;
                }
                //
                // found assembly
                //
                try
                {
                    foreach (var t in a.GetTypes())
                    {
                        if(t.FullName != methodInfo.InterfaceName)
                        {
                            continue;
                        }
                        //
                        // found interface
                        //
                        foreach(var m in t.GetMethods())
                        {
                            if (m.Name != methodInfo.MethodName)
                            {
                                continue;
                            }
                            var parameters = m.GetParameters();
                            if (parameters.Length != methodInfo.Arguments.Count())
                            {
                                continue;
                            }
                            //
                            // found method
                            //
                            var matches = true;
                            int idx = 0;
                            foreach(var arg in methodInfo.Arguments)
                            {
                                if (arg.Name != parameters[idx].Name)
                                {
                                    matches = false;
                                }
                                if (arg.ValueType != SolidRpcTypeStore.GetTypeName(parameters[idx].ParameterType))
                                {
                                    matches = false;
                                }
                            }
                            if (!matches)
                            {
                                continue;
                            }

                            //
                            // found it!
                            // 
                            return m;
                        }
                    }
                }
                catch
                {

                }
            }
            return null;
        }
    }
}
