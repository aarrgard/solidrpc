using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Contains extension methods for the Type
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly FileTypeHelper s_NotFileType = new FileTypeHelper(false, 
            null, null, 
            null, null,
            null, null,
            null, null,
            null, null
        );

        /// <summary>
        /// The file type properties that can exist on a file type.
        /// </summary>
        public static IDictionary<string, Type> FileTypeProperties = new Dictionary<string, Type>() {
            { "contenttype", typeof(string) },
            { "filename", typeof(string) },
            { "lastmodified", typeof(DateTime?) },
            { "charset", typeof(string) },
        };

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
                Func<object, string> getFilename,
                Action<object, string> setCharSet,
                Func<object, string> getCharSet,
                Action<object, DateTime?> setLastModified,
                Func<object, DateTime?> getLastModified
                )
            {
                IsFileType = isFileType;
                SetStreamData = setStreamData;
                GetStreamData = getStreamData;
                SetContentType = setContentType;
                GetContentType = getContentType;
                SetFilename = setFilename;
                GetFilename = getFilename;
                SetCharSet = setCharSet;
                GetCharSet = getCharSet;
                SetLastModified = setLastModified;
                GetLastModified = getLastModified;
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

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Action<object, string> SetCharSet { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Func<object, string> GetCharSet { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Action<object, DateTime?> SetLastModified { get; }

            /// <summary>
            /// The method we use to set the stream data.
            /// </summary>
            public Func<object, DateTime?> GetLastModified { get; }
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

            var res = propertyTypes
                .Where(o => {
                    if(FileTypeProperties.TryGetValue(o.Key.ToLower(), out Type t))
                    {
                        return t == o.Value;
                    }
                    return false;
                })
                .Union(new [] { dataProp })
                .ToList();

            return res.Count == propertyTypes.Count;
        }

        private static FileTypeHelper CreateFileTypeHelper(Type arg)
        {
            if(arg.IsTaskType(out Type taskType))
            {
                arg = taskType;
            }
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
            if (filenameProp != null)
            {
                setFilename = (impl, fileName) => filenameProp.SetValue(impl, fileName);
                getFilename = (impl) => (string)filenameProp.GetValue(impl);
            }

            //
            // handle charset
            //
            var charsetProp = arg.GetProperties()
                .Where(o => string.Equals(o.Name, "charset", StringComparison.InvariantCultureIgnoreCase))
                .Where(o => o.PropertyType == typeof(string))
                .FirstOrDefault();
            Action<object, string> setCharset = (impl, charset) => { };
            Func<object, string> getCharset = (impl) => { return null; };
            if (charsetProp != null)
            {
                setCharset = (impl, charset) => charsetProp.SetValue(impl, charset);
                getCharset = (impl) => (string)charsetProp.GetValue(impl);
            }

            //
            // handle last modified
            //
            var lastModifiedProp = arg.GetProperties()
                .Where(o => string.Equals(o.Name, "lastmodified", StringComparison.InvariantCultureIgnoreCase))
                .Where(o => o.PropertyType == typeof(DateTime?))
                .FirstOrDefault();
            Action<object, DateTime?> setLastModified = (impl, dt) => { };
            Func<object, DateTime?> getLastModified = (impl) => { return null; };
            if (lastModifiedProp != null)
            {
                setLastModified = (impl, dt) => lastModifiedProp.SetValue(impl, dt);
                getLastModified = (impl) => (DateTime?)lastModifiedProp.GetValue(impl);
            }

            var remainingProps = arg.GetProperties()
                .Where(o => o != dataProp)
                .Where(o => o != contentTypeProp)
                .Where(o => o != filenameProp)
                .Where(o => o != charsetProp)
                .Where(o => o != lastModifiedProp)
                .ToList();

            if(remainingProps.Count != 0)
            {
                return s_NotFileType;
            }

            return new FileTypeHelper(true,
                setStreamData, getStreamData,
                setContentType, getContentType,
                setFilename, getFilename,
                setCharset, getCharset,
                setLastModified, getLastModified
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
        public static void SetFileTypeStreamData(this Type type, object impl, Stream data)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            h.SetStreamData(impl, data);
        }

        /// <summary>
        /// Set the stream data on supplied structure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <param name="contentType"></param>
        public static void SetFileTypeContentType(this Type type, object impl, string contentType)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            h.SetContentType(impl, contentType);
        }

        /// <summary>
        /// Set the stream data on supplied structure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <param name="fileName"></param>
        public static void SetFileTypeFilename(this Type type, object impl, string fileName)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            h.SetFilename(impl, fileName);
        }

        /// <summary>
        /// Set the stream data on supplied structure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <param name="charSet"></param>
        public static void SetFileTypeCharSet(this Type type, object impl, string charSet)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            h.SetCharSet(impl, charSet);
        }

        /// <summary>
        /// Set the stream data on supplied structure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <param name="lastModified"></param>
        public static void SetFileTypeLastModified(this Type type, object impl, DateTime? lastModified)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            h.SetLastModified(impl, lastModified);
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

        /// <summary>
        /// Returns the filename
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <returns></returns>
        public static string GetFileTypeFilename(this Type type, object impl)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            return h.GetFilename(impl);
        }

        /// <summary>
        /// Returns the charset
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <returns></returns>
        public static string GetFileTypeCharSet(this Type type, object impl)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            return h.GetCharSet(impl);
        }

        /// <summary>
        /// Returns the last modified
        /// </summary>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <returns></returns>
        public static DateTime? GetFileTypeLastModified(this Type type, object impl)
        {
            var h = s_FileTypes.GetOrAdd(type, CreateFileTypeHelper);
            if (!h.IsFileType) throw new Exception("Type is not a file type:" + type.FullName);
            return h.GetLastModified(impl);
        }

        /// <summary>
        /// Returns true if the type is an enum
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static bool GetEnumType(this Type type, out Type enumType)
        {
            if (!type.IsGenericType)
            {
                enumType = null;
                return false;
            }
            IEnumerable<Type> interfaceProspects = type.GetInterfaces();
            if(type.IsInterface)
            {
                interfaceProspects = interfaceProspects.Union(new[] { type });
            }
            var enumInterface = interfaceProspects
                .Where(o => o.IsGenericType)
                .Where(o => o.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .FirstOrDefault();
            if (enumInterface == null)
            {
                enumType = null;
                return false;
            }
            enumType = enumInterface.GetGenericArguments()[0];
            return true;
        }

        /// <summary>
        /// Returns true if supplied tyoe is a task type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public static bool IsTaskType(this Type type, out Type taskType)
        {
            if (!type.IsGenericType)
            {
                taskType = null;
                return false;
            }
            if (!typeof(Task<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                taskType = null;
                return false;
            }
            taskType = type.GetGenericArguments()[0];
            return true;
        }

        /// <summary>
        /// Returns true if supplied type is a nullable type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nullableType"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type, out Type nullableType)
        {
            if (!type.IsGenericType)
            {
                nullableType = null;
                return false;
            }
            if (!typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                nullableType = null;
                return false;
            }
            nullableType = type.GetGenericArguments()[0];
            return true;
        }
    }
}
