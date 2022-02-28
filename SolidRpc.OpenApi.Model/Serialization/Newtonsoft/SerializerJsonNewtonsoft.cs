using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SolidRpc.Abstractions.Serialization;
using System;
using System.IO;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    public class SerializerJsonNewtonsoft : ISerializer
    {
        public SerializerJsonNewtonsoft(ISerializerFactory factory, SerializerSettings serializerSettings)
        {
            Factory = factory;
            SerializerSettings = serializerSettings;
            JsonSerializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Formatting = serializerSettings.PrettyPrint ? Formatting.Indented : Formatting.None,
                ContractResolver = new NewtonsoftContractResolver(serializerSettings)
            });
            JsonSerializer.Converters.Add(new StringEnumConverter() { AllowIntegerValues = false });
        }

        private ISerializerFactory Factory { get; }

        private JsonSerializer JsonSerializer { get; }

        public SerializerSettings SerializerSettings { get; }

        public void Deserialize(Stream stream, Type type, out object o)
        {
            using (var tr = CreateTextReader(stream))
            using (var jtr = new JsonTextReader(tr))
            {
                o = JsonSerializer.Deserialize(jtr, type);
            }
        }

        public void Serialize(Stream stream, Type type, object o)
        {
            using (var tw = CreateTextWriter(stream))
            using (var jtw = new JsonTextWriter(tw))
            {
                JsonSerializer.Serialize(jtw, o, type);
            }
        }

        private TextWriter CreateTextWriter(Stream stream)
        {
            if (SerializerSettings.CharSet == null)
            {
                return new StreamWriter(stream);
            }
            else
            {
                return new StreamWriter(stream, SerializerSettings.CharSet);
            }
        }

        private TextReader CreateTextReader(Stream stream)
        {
            if (SerializerSettings.CharSet == null)
            {
                return new StreamReader(stream);
            }
            else
            {
                return new StreamReader(stream, SerializerSettings.CharSet);
            }
        }
    }
}
