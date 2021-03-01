using System.Runtime.Serialization;

namespace SolidRpc.NpmGenerator.Debugger
{
    public class V8DebuggerCommand
    {
        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "method")]
        public string MethodName { get; set; }

        [DataMember(Name = "params", EmitDefaultValue = false)]
        public object Parameters { get; set; }
    }
}
