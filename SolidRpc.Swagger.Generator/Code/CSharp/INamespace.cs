namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Represents an interface
    /// </summary>
    public interface INamespace : IQualifiedMember
    {
        /// <summary>
        /// Returns the interface in this namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IInterface GetInterface(string name);

        /// <summary>
        /// Returns the class in this namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IClass GetClass(string name);

        /// <summary>
        /// Returns the namespace for supplied name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        INamespace GetNamespace(string name);
    }
}