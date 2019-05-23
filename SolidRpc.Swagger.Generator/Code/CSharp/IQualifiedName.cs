namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public interface IQualifiedMember : IMember
    {
        /// <summary>
        /// Returns the fully qualified name
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Returns the namespace
        /// </summary>
        string Namespace { get; }
    }
}