using Microsoft.Azure.WebJobs.Description;
using System;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// Represents a constant value
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ConstantAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new attribute with supplied value
        /// </summary>
        /// <param name="value"></param>
        public ConstantAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; }
    }
}
