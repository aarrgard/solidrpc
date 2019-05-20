using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public interface IMethod : IMember
    {
        /// <summary>
        /// The return type.
        /// </summary>
        IClass ReturnType { get; set; }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        IParameter AddParameter(string parameterName);

        /// <summary>
        /// The parameters.
        /// </summary>
        IEnumerable<IParameter> Parameters { get; }
    }
}