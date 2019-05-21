namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Represents an interface
    /// </summary>
    public interface IInterface : IMember
    {

        /// <summary>
        /// Returns the fully qualified name
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Adds a method to this interface
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        IMethod AddMethod(string methodName);
    }
}