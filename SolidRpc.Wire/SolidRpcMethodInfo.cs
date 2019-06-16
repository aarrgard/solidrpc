using System.Collections.Generic;

namespace SolidRpc.Wire
{
    /// <summary>
    /// Contains information about the method we are invoking.
    /// </summary>
    public class SolidRpcMethodInfo
    {
        /// <summary>
        /// The api name(AsseblyName)
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// The api version.
        /// </summary>
        public string ApiVersion { get; set; }
        
        /// <summary>
        /// The interface that the method belongs to.
        /// </summary>
        public string InterfaceName { get; set; }

        /// <summary>
        /// The method name.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The method arguments.
        /// </summary>
        public IEnumerable<SolidRpcMethodArg> Arguments { get; set; }

        /// <summary>
        /// The return type.
        /// </summary>
        public string ReturnType { get; set; }
    }
}