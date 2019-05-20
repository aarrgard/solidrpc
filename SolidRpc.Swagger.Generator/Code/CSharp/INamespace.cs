namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Represents an interface
    /// </summary>
    public interface INamespace : IMember
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
    }
}