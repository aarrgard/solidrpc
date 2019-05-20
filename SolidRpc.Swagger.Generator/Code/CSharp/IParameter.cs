namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public interface IParameter : IMember
    {
        /// <summary>
        /// The parameter type.
        /// </summary>
        IClass ParameterType { get; set; }
    }
}