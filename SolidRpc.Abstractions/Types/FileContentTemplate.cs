using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a file content
    /// </summary>
    public class FileContentTemplate : TypeTemplate<FileContent>
    {
        static FileContentTemplate()
        {
            PropertyTypes = typeof(FileContent).GetProperties().ToDictionary(o => o.Name.ToLower(), o => o.PropertyType);
            RequiredProps = new[] { nameof(FileContent.Content).ToLower() };
        }

        /// <summary>
        /// The properties 
        /// </summary>
        public static IDictionary<string, Type> PropertyTypes { get; }

        /// <summary>
        /// The required properties
        /// </summary>
        public static IEnumerable<string> RequiredProps { get; }

        /// <summary>
        /// Determines if the supplied type is a file type based on the name and/or properties.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyTypes"></param>
        /// <returns></returns>
        public static bool IsFileType(string typeName, Dictionary<string, Type> propertyTypes)
        {
            if (typeName == typeof(Stream).FullName)
            {
                return true;
            }
            if(!RequiredProps.All(o => propertyTypes.Keys.Any(o2 => o == o2.ToLower())))
            {
                return false;
            }

            var res = propertyTypes
                .All(o => {
                    if (PropertyTypes.TryGetValue(o.Key.ToLower(), out Type t))
                    {
                        return t == o.Value;
                    }
                    return false;
                });

            return res;
        }

        /// <summary>
        /// REturns the template spec for supplied type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FileContentTemplate GetTemplate(Type type)
        {
            return (FileContentTemplate)GetTemplate(type,  _ => new FileContentTemplate(_, RequiredProps));
        }

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="otherType"></param>
        /// <param name="requiredProps"></param>
        public FileContentTemplate(Type otherType, IEnumerable<string> requiredProps)
            : base(otherType, requiredProps)
        {
        }

        /// <summary>
        /// Adds support for plain streams
        /// </summary>
        /// <param name="templateType"></param>
        /// <param name="otherType"></param>
        /// <param name="isTemplateType"></param>
        /// <returns></returns>
        protected override bool HandleTemplateType(Type templateType, Type otherType, bool isTemplateType)
        {
            if(otherType == typeof(Stream))
            {
                SetContent = (o1, o2) =>
                {
                    var s = (Stream)o1;
                    var pos = s.Position;
                    o2.CopyTo(s);
                    s.Position = pos;
                };
                GetContent = (_) => (Stream)_;
                GetContentType = (_) => "application/octet-stream";
                return true;
            }
            return isTemplateType;
        }

        /// <summary>
        /// Sets the content on the template copy
        /// </summary>
        public Action<object, Stream> SetContent { get; private set; }
        /// <summary>
        /// Gets the content from the template copy
        /// </summary>
        public Func<object, Stream> GetContent { get; private set; }
        /// <summary>
        /// Sets the charset on the template copy
        /// </summary>
        public Action<object, string> SetCharSet { get; private set; }
        /// <summary>
        /// Gets the charset from the template copy
        /// </summary>
        public Func<object, string> GetCharSet { get; private set; }
        /// <summary>
        /// Sets the content type on the template  copy
        /// </summary>
        public Action<object, string> SetContentType { get; private set; }
        /// <summary>
        /// Returns the content type from the template copy
        /// </summary>
        public Func<object, string> GetContentType { get; private set; }
        /// <summary>
        /// Sets the filename on the template copy
        /// </summary>
        public Action<object, string> SetFileName { get; private set; }
        /// <summary>
        /// Returns the filename from the template copy
        /// </summary>
        public Func<object, string> GetFileName { get; private set; }
        /// <summary>
        /// Sets the last modified on the template copy
        /// </summary>
        public Action<object, DateTimeOffset?> SetLastModified { get; private set; }
        /// <summary>
        /// Returns the last modified from the template copy
        /// </summary>
        public Func<object, DateTimeOffset?> GetLastModified { get; private set; }
        /// <summary>
        /// Sets the location in the template copy
        /// </summary>
        public Action<object, string> SetLocation { get; private set; }
        /// <summary>
        /// Returns the location from the template copy
        /// </summary>
        public Func<object, string> GetLocation { get; private set; }
    }
}
