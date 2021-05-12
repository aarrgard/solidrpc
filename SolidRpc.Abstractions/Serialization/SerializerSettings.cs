using System;
using System.Text;

namespace SolidRpc.Abstractions.Serialization
{
    /// <summary>
    /// Represents the serializer settings
    /// </summary>
    public class SerializerSettings
    {
        public static readonly SerializerSettings Default = new SerializerSettings("application/json", null, TimeZoneInfo.Local, false);

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="charSet"></param>
        /// <param name="defaultTimeZone"></param>
        /// <param name="prettyPrint"></param>
        public SerializerSettings(string contentType, Encoding charSet, TimeZoneInfo defaultTimeZone, bool prettyPrint)
        {
            ContentType = contentType;
            CharSet = charSet;
            DefaultTimeZone = defaultTimeZone;
            PrettyPrint = prettyPrint;

            UniqueKey = $"{ContentType}.{CharSet?.HeaderName}.{DefaultTimeZone.Id}.{PrettyPrint}";
        }

        /// <summary>
        /// The unique key tata identifies this settings
        /// </summary>
        public string UniqueKey { get; }

        /// <summary>
        /// Returns the content type for this serializer
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// The charset for this serializer
        /// </summary>
        public Encoding CharSet { get; }


        /// <summary>
        /// The default time zone to use when deserializing dates.
        /// </summary>
        public TimeZoneInfo DefaultTimeZone { get; }

        /// <summary>
        /// Should the formatting be pretty - defaults to false
        /// </summary>
        public bool PrettyPrint { get; } = false;

        /// <summary>
        /// Sets the content type of the settings
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public SerializerSettings SetContentType(string contentType)
        {
            if (ContentType == contentType)
            {
                return this;
            }
            return new SerializerSettings(contentType, CharSet, DefaultTimeZone, PrettyPrint);
        }

        /// <summary>
        /// Sets the charset of the settings
        /// </summary>
        /// <param name="charSet"></param>
        /// <returns></returns>
        public SerializerSettings SetCharSet(Encoding charSet)
        {
            if (CharSet == charSet)
            {
                return this;
            }
            return new SerializerSettings(ContentType, charSet, DefaultTimeZone, PrettyPrint);
        }

        /// <summary>
        /// Sets the pretty print of the settings
        /// </summary>
        /// <param name="prettyPrint"></param>
        /// <returns></returns>
        public SerializerSettings SetPrettyPrint(bool prettyPrint)
        {
            if (PrettyPrint == prettyPrint)
            {
                return this;
            }
            return new SerializerSettings(ContentType, CharSet, DefaultTimeZone, prettyPrint);
        }

        /// <summary>
        /// Sets the default time zone
        /// </summary>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public SerializerSettings SetDefaultTimeZone(TimeZoneInfo timeZone)
        {
            if (DefaultTimeZone.Id == timeZone.Id)
            {
                return this;
            }
            return new SerializerSettings(ContentType, CharSet, timeZone, PrettyPrint);
        }
    }
}