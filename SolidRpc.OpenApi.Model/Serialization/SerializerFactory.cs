using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Model.Serialization;
using SolidRpc.OpenApi.Model.Serialization.Newtonsoft;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

[assembly: SolidRpcService(typeof(ISerializerFactory), typeof(SerializerFactory))]
namespace SolidRpc.OpenApi.Model.Serialization
{
    public class SerializerFactory : ISerializerFactory
    {
        private ConcurrentDictionary<string, ISerializer> _serializers = new ConcurrentDictionary<string, ISerializer>();

        public SerializerFactory()
        {
            DefaultSerializerSettings = SerializerSettings.Default;
        }


        public SerializerSettings DefaultSerializerSettings { get; set; }

        public void SerializeToString<T>(out string s, T o, string contentType, Encoding charSet = null, bool prettyFormat = false)
        {
            SerializeToString(out s, typeof(T), o, contentType, charSet, prettyFormat);
        }

        public void DeserializeFromString<T>(string s, out T t, string contentType, Encoding charSet = null)
        {
            object o;
            DeserializeFromString(s, typeof(T), out o, contentType, charSet);
            t = (T)o;
        }

        public void SerializeToStream<T>(Stream s, T o, string contentType, Encoding charSet = null, bool prettyFormat = false)
        {
            SerializeToStream(s, typeof(T), o, contentType, charSet, prettyFormat);
        }

        public void DeserializeFromStream<T>(Stream s, out T t, string contentType, Encoding charSet = null)
        {
            object o;
            DeserializeFromStream(s, typeof(T), out o, contentType, charSet);
            t = (T)o;
        }

        public void SerializeToStream(Stream s, Type type, object o, string contentType, Encoding charSet = null, bool prettyFormat = false)
        {
            GetSerializer(contentType, charSet, prettyFormat).Serialize(s, type, o);
        }

        public void DeserializeFromStream(Stream s, Type type, out object o, string contentType, Encoding charSet = null)
        {
            GetSerializer(contentType, charSet).Deserialize(s, type, out o);
        }

        public void SerializeToString(out string s, Type type, object o, string contentType, Encoding charSet = null, bool prettyFormat = false)
        {
            using (var ms = new MemoryStream())
            {
                Encoding enc;
                using (var sw = new StreamWriter(ms))
                {
                    enc = sw.Encoding;
                    SerializeToStream(ms, type, o, contentType, charSet, prettyFormat);
                }
                s = enc.GetString(ms.ToArray());
            }
        }

        public void DeserializeFromString(string s, Type type, out object o, string contentType, Encoding charSet = null)
        {
            charSet = charSet ?? Encoding.UTF8;
            using (var ms = new MemoryStream(charSet.GetBytes(s)))
            {
                DeserializeFromStream(ms, type, out o, contentType, charSet);
            }
        }

        public ISerializer GetSerializer(string contentType, Encoding charSet = null, bool prettyPrint = false)
        {
            var serializerSettings = DefaultSerializerSettings
                .SetContentType(contentType)
                .SetCharSet(charSet)
                .SetPrettyPrint(prettyPrint);

            return _serializers.GetOrAdd(serializerSettings.UniqueKey, _ =>
            {
                switch(serializerSettings.ContentType)
                {
                    case "application/json":
                        return new SerializerJsonNewtonsoft(this, serializerSettings);
                    default:
                        throw new Exception("Cannot create serializer for content type:" + serializerSettings.ContentType);
                }
            });
        }

        public void SerializeToFileType<T>(object fileTypeInstance, T o, string mediaType = "application/json", Encoding charSet = null, bool prettyFormat = false)
        {
            SerializeToFileType(fileTypeInstance, typeof(T), o, mediaType, charSet, prettyFormat);
        }

        public void SerializeToFileType(object fileTypeInstance, Type type, object o, string mediaType, Encoding charSet = null, bool prettyFormat = false)
        {
            var fileType = fileTypeInstance?.GetType() ?? typeof(object);
            var template = FileContentTemplate.GetTemplate(fileType);
            if (!template.IsTemplateType)
            {
                throw new ArgumentException("Supplied file type instance is not a file type");
            }
            var ms = new MemoryStream();
            SerializeToStream(ms, type, o, mediaType, charSet, prettyFormat);
            template.SetContent(fileTypeInstance, new MemoryStream(ms.ToArray()));
            template.SetFileName(fileTypeInstance, SolidRpcTypeStore.GetTypeName(type, true));
            template.SetContentType(fileTypeInstance, mediaType);
            template.SetCharSet(fileTypeInstance, charSet?.HeaderName);

        }

        public void DeserializeFromFileType<T>(object fileTypeInstance, out T o)
        {
            var fileType = fileTypeInstance?.GetType() ?? typeof(object);
            var template = FileContentTemplate.GetTemplate(fileType);
            if (!template.IsTemplateType)
            {
                throw new ArgumentException("Supplied file type instance is not a file type");
            }
            var fileName = template.GetFileName(fileTypeInstance);
            var typeName = SolidRpcTypeStore.GetTypeName(typeof(T), true);
            if (fileName != typeName)
            {
                throw new ArgumentException($"Filename({fileName}) of supplied file instance does not match type({typeName}) to deserialize");
            }
            var mediaType = template.GetContentType(fileTypeInstance);
            var charSet = template.GetCharSet(fileTypeInstance);
            var encoding = charSet == null ? null : Encoding.GetEncoding(charSet);
            var stream = template.GetContent(fileTypeInstance);
            object tmp;
            DeserializeFromStream(stream, typeof(T), out tmp, mediaType, encoding);
            o = (T)tmp;
        }
    }
}
