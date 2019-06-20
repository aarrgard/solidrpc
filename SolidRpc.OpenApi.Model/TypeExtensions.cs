using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// Contains extension methods for the Type
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly FileTypeHelper s_NotFileType = new FileTypeHelper(false, null, null, null, null,null,null);

        /// <summary>
        /// Contains information about a file type
        /// </summary>
        private class FileTypeHelper
        {
            public FileTypeHelper(
                bool isFileType,
                Action<object, Stream> setStreamData,
                Func<object, Stream> getStreamData,
                Action<object, string> setContentType,
                Func<object, string> getContentType,
                Action<object, string> setFilename,
                Func<object, string> getFilename
                )
            {
                IsFileType = isFileType;
                SetStreamData = setStreamData;
                GetStreamData = getStreamData;
                SetContentType = setContentType;
                GetContentType = getContentType;
                SetFilename = setFilename;
                GetFilename = getFilename;
            }
            /// <summary>
            /// Returns true if this type is a file type.
            /// </summary>
            public bool IsFileType { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Action<object, Stream> SetStreamData { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Func<object, Stream> GetStreamData { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Action<object, string> SetContentType { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Func<object, string> GetContentType { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Action<object, string> SetFilename { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Func<object, string> GetFilename { get; }
        }
        private static ConcurrentDictionary<Type, FileTypeHelper> s_FileTypes = new ConcurrentDictionary<Type, FileTypeHelper>();

        /// <summary>
        /// Returns true if the type is a file type.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyTypes"></param>
        /// <returns></returns>
        public static bool IsFileType(string typeName, Dictionary<string, Type> propertyTypes)
        {
            if(typeName == typeof(Stream).FullName)
            {
                return true;
            }
            var dataProp = propertyTypes.Where(o => typeof(Stream).IsAssignableFrom(o.Value) || typeof(byte[]).IsAssignableFrom(o.Value)).FirstOrDefault();
            if(dataProp.Key == null)
            {
                return false;
            }
            var contentTypeProp = propertyTypes.Where(o => string.Equals(o.Key, "contenttype", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var fileNameProp = propertyTypes.Where(o => string.Equals(o.Key, "filename", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            var keys = new[] { dataProp.Key, contentTypeProp.Key, fileNameProp.Key };
            var isFileType = propertyTypes.Keys.All(o => keys.Contains(o));
            if(isFileType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static FileTypeHelper CreateFileTypeHelper(Type arg)
        {
            //
            // we need an empty constructor
            //
            if(arg.GetConstructors().Where(o => o.GetParameters().Length == 0).Count() != 1)
            {
                return s_NotFileType;
            }

            var propertyTypes = arg.GetProperties().ToDictionary(o => o.Name, o => o.PropertyType);
            if(!IsFileType(arg.FullName, propertyTypes))
            {
                return s_NotFileType;
            }

            // we need a property of "Stream" or "Byte" type with setter and getter
            var dataProps = arg.GetProperties()
                .Where(o => typeof(Stream).IsAssignableFrom(o.PropertyType) || typeof(byte[]).IsAssignableFrom(o.PropertyType))
                .ToList();

            if (dataProps.Count != 1)
            {
                return s_NotFileType;
            }

            //
            // handle stream or byte[]
            //
            Action<object, Stream> setStreamData;
            Func<object, Stream> getStreamData;
            var dataProp = dataProps.Single();
            if(typeof(byte[]).IsAssignableFrom(dataProp.PropertyType))
            {
                setStreamData = (impl, stream) =>
                {
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    dataProp.SetValue(impl, ms.ToArray());
                };
                getStreamData = (impl) =>
                {
                    var data = (byte[]) dataProp.GetValue(impl);
                    var ms = new MemoryStream(data);
                    return ms;
                };
            }
            else if (typeof(Stream).IsAssignableFrom(dataProp.PropertyType))
            {
                setStreamData = (impl, stream) =>
                {
                    dataProp.SetValue(impl, stream);
                };
                getStreamData = (impl) =>
                {
                    return (Stream)dataProp.GetValue(impl);
                };
            }
            else
            {
                throw new Exception();
            }

            //
            // handle content type
            //
            var contentTypeProp = arg.GetProperties()
                .Where(o => string.Equals(o.Name, "contenttype", StringComparison.InvariantCultureIgnoreCase))
                .Where(o => o.PropertyType == typeof(string))
                .FirstOrDefault();
            Action<object, string> setContentType = (impl, filename) => { };
            Func<object, string> getContentType = (impl) => { return null; };
            if (contentTypeProp != null)
            {
                setContentType = (impl, contentType) => contentTypeProp.SetValue(impl, contentType);
                getContentType = (impl) => (string)contentTypeProp.GetValue(impl);
            }

            //
            // handle filename
            //
            var filenameProp = arg.GetProperties()
                .Where(o => string.Equals(o.Name, "filename", StringComparison.InvariantCultureIgnoreCase))
                .Where(o => o.PropertyType == typeof(string))
                .FirstOrDefault();
            Action<object, string> setFilename = (impl, filename) => { };
            Func<object, string> getFilename = (impl) => { return null; };
            if (contentTypeProp != null)
            {
                setFilename = (impl, contentType) => filenameProp.SetValue(impl, contentType);
                getFilename = (impl) => (string)filenameProp.GetValue(impl);
            }

            var remainingProps = arg.GetProperties()
                .Where(o => o != dataProp)
                .Where(o => o != contentTypeProp)
                .Where(o => o != filenameProp)
                .ToList();

            if(remainingProps.Count != 0)
            {
                return s_NotFileType;
            }

            return new FileTypeHelper(true,
                setStreamData, getStreamData,
                setContentType, getContentType,
                setFilename, getFilename
                );
        }

        /// <summary>
        /// Returns true if the supplied type is a  file type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFileType(this Type type)
        {
            return s_FileTypes.GetOrAdd(type, CreateFileTypeHelper).IsFileType;
        }

        /// <summary>
        /// Set the stream data on supplied structure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <param name="data"></param>
        public static void SetStreamData(this Type type, object impl, Stream data)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            h.SetStreamData(impl, data);
        }

        /// <summary>
        /// Returns the stream data.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <returns></returns>
        public static Stream GetFileTypeStreamData(this Type type, object impl)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            return h.GetStreamData(impl);
        }

        /// <summary>
        /// Returns the content type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <returns></returns>
        public static string GetFileTypeContentType(this Type type, object impl)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            return h.GetContentType(impl);
        }
    }
}
