namespace SolidRpc.OpenApi.Generator.Model.CSharp
{
    /// <summary>
    /// Represents a c# method parameter.
    /// </summary>
    public interface ICSharpMethodParameter : ICSharpMember
    {
        /// <summary>
        /// Specifies if this parameter is optional.
        /// </summary>
        bool Optional { get; }

        /// <summary>
        /// The parameter type.
        /// </summary>
        ICSharpType ParameterType { get; }
    }
}