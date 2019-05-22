namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public interface IParameter : IMember
    {
        /// <summary>
        /// The parameter description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The parameter type.
        /// </summary>
        IClass ParameterType { get; set; }

        /// <summary>
        /// The default value for the parameter.
        /// </summary>
        string DefaultValue { get; set; }
    }
}