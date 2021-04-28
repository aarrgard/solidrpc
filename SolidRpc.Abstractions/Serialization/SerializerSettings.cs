using System;

namespace SolidRpc.Abstractions.Serialization
{
    /// <summary>
    /// Represents the serializer settings
    /// </summary>
    public class SerializerSettings
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="defaultTimeZone"></param>
        /// <param name="prettyPrint"></param>
        public SerializerSettings(TimeZoneInfo defaultTimeZone, bool prettyPrint)
        {
            DefaultTimeZone = defaultTimeZone;
            PrettyPrint = prettyPrint;
        }

        /// <summary>
        /// The default time zone to use when deserializing dates.
        /// </summary>
        public TimeZoneInfo DefaultTimeZone { get; }

        /// <summary>
        /// Should the formatting be pretty - defaults to false
        /// </summary>
        public bool PrettyPrint { get; } = false;
    }
}