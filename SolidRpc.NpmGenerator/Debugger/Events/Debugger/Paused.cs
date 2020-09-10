using Newtonsoft.Json;

namespace SolidRpc.NpmGenerator.Debugger.Events.Debugger
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
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
