using Newtonsoft.Json;

namespace SolidRpc.NpmGenerator.Debugger
{
    public class V8DebuggerResponse
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }

        [JsonProperty("method")]
        public string MethodName { get; set; }

        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public object Parameters { get; set; }
    }
}
