using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    [Extension("Constant")]
    public class ConstantExtensionConfigProvider : IExtensionConfigProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public class ConstantConverterString : IConverter<ConstantAttribute, string>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public string Convert(ConstantAttribute input)
            {
                return input.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ConstantConverterType : IConverter<ConstantAttribute, Type>
        {
            /// <summary>
            /// 
            /// </summary>
            private ConcurrentDictionary<string, Type> _types = new ConcurrentDictionary<string, Type>();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public Type Convert(ConstantAttribute input)
            {
                return _types.GetOrAdd(input.Value, FindType);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            private Type FindType(string typeName)
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(o => o.GetTypes())
                    .Where(o => o.FullName == typeName)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<ConstantAttribute>().BindToInput(new ConstantConverterString());
            context.AddBindingRule<ConstantAttribute>().BindToInput(new ConstantConverterType());
        }
    }
}
