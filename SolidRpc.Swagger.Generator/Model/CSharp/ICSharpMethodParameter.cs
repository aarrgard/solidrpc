namespace SolidRpc.Swagger.Generator.Model.CSharp
{
    /// <summary>
    /// Represents a c# method parameter.
    /// </summary>
    public interface ICSharpMethodParameter : ICSharpMember
    {
        /// <summary>
        /// The parameter type.
        /// </summary>
        ICSharpType ParameterType { get; }
    }
}