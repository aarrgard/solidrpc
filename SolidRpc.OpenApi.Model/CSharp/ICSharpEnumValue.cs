namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a c# enum value
    /// </summary>
    public interface ICSharpEnumValue : ICSharpMember
    {
        /// <summary>
        /// The enum value
        /// </summary>
        int? Value { get; }
    }
}