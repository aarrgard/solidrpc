namespace SolidRpc.Wire
{
    /// <summary>
    /// Represents a method argument
    /// </summary>
    public class SolidRpcMethodArg
    {
        /// <summary>
        /// The name of the argument
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value type.
        /// </summary>
        public string ValueType { get; set; }
    }
}