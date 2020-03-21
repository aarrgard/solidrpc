using Newtonsoft.Json;
using SolidRpc.Abstractions.Serialization;
using System;
using System.IO;
using System.Text;

namespace SolidRpc.OpenApi.Model.Serialization.Newtonsoft
{
    public class SerializerJsonNewtonsoft : ISerializer
    {
        private static readonly JsonSerializerSettings s_settingsCompact = new JsonSerializerSettings()
        {
            Formatting = Formatting.None,
            ContractResolver = NewtonsoftContractResolver.Instance
        };
        private static readonly JsonSerializerSettings s_settingsFormatted = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = NewtonsoftContractResolver.Instance
        };

        public SerializerJsonNewtonsoft(ISerializerFactory factory, string contentType, Encoding charSet)
        {
            Factory = factory;
            ContentType = contentType;
            CharSet = charSet;
            JsonSerializerCompact = JsonSerializer.Create(s_settingsCompact);
        }

        private ISerializerFactory Factory { get; }

        public string ContentType { get; }

        public Encoding CharSet { get; }

        private JsonSerializer JsonSerializerCompact { get; }

        public void Deserialize(Stream stream, Type type, out object o)
        {
            using (var tr = CreateTextReader(stream))
            using (var jtr = new JsonTextReader(tr))
            {
                o = JsonSerializerCompact.Deserialize(jtr, type);
            }
        }

        public void Serialize(Stream stream, Type type, object o, bool pretty = false)
        {
            using (var tw = CreateTextWriter(stream))
            using (var jtw = new JsonTextWriter(tw))
            {
                var jsonSerializer = JsonSerializerCompact;
                if (pretty)
                {
                    jsonSerializer = JsonSerializer.Create(s_settingsFormatted);
                }
                jsonSerializer.Serialize(jtw, o, type);
            }
        }

        private TextWriter CreateTextWriter(Stream stream)
        {
            if (CharSet == null)
            {
                return new StreamWriter(stream);
            }
            else
            {
                return new StreamWriter(stream, CharSet);
            }
        }

        private TextReader CreateTextReader(Stream stream)
        {
            if (CharSet == null)
            {
                return new StreamReader(stream);
            }
            else
            {
                return new StreamReader(stream, CharSet);
            }
        }
    }
}
