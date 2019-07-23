using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    [Extension("Constant")]
    public class ConstantExtensionConfigProvider : IExtensionConfigProvider
    {
        public class ConstantConverterString : IConverter<ConstantAttribute, string>
        {
            public string Convert(ConstantAttribute input)
            {
                return input.Value;
            }
        }

        public class ConstantConverterType : IConverter<ConstantAttribute, Type>
        {
            private ConcurrentDictionary<string, Type> _types = new ConcurrentDictionary<string, Type>();
            public Type Convert(ConstantAttribute input)
            {
                return _types.GetOrAdd(input.Value, FindType);
            }

            private Type FindType(string typeName)
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(o => o.GetTypes())
                    .Where(o => o.FullName == typeName)
                    .FirstOrDefault();
            }
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<ConstantAttribute>().BindToInput(new ConstantConverterString());
            context.AddBindingRule<ConstantAttribute>().BindToInput(new ConstantConverterType());
        }
    }
}
