using System.Runtime.Serialization;

namespace SolidRpc.Node.Debugger.Events.Debugger
{
    /// <summary>
    /// 
    /// </summary>
    public class Paused
    {
        /// <summary>
        /// Pause reason.
        /// Allowed Values: ambiguous, assert, debugCommand, DOM, EventListener, exception, instrumentation, OOM, other, promiseRejection, XHR
        /// </summary>
        [DataMember(Name = "reason")]
        public string Reason { get; set; }
    }
}
