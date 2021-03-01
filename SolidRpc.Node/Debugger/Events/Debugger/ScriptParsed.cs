using System.Runtime.Serialization;

namespace SolidRpc.Node.Debugger.Events.Debugger
{
    /// <summary>
    /// Fired when virtual machine parses script. This event is also fired for all known and uncollected scripts upon enabling debugger.
    /// </summary>
    public class ScriptParsed
    {
        /// <summary>
        /// Identifier of the script parsed.
        /// </summary>
        [DataMember(Name = "scriptId")]
        public string ScriptId { get; set; }

        /// <summary>
        /// URL or name of the script parsed (if any).
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Line offset of the script within the resource with given URL (for script tags).
        /// </summary>
        [DataMember(Name = "startLine")]
        public int StartLine { get; set; }

        /// <summary>
        /// Column offset of the script within the resource with given URL.
        /// </summary>
        [DataMember(Name = "startColumn")]
        public int StartColumn { get; set; }

        /// <summary>
        /// Last line of the script.
        /// </summary>
        [DataMember(Name = "endLine")]
        public int EndLine { get; set; }

        /// <summary>
        /// Length of the last line of the script.
        /// </summary>
        [DataMember(Name = "endColumn")]
        public int EndColumn { get; set; }

        /// <summary>
        /// Specifies script creation context.
        /// </summary>
        [DataMember(Name = "executionContextId")]
        public int ExecutionContextId { get; set; }

        /// <summary>
        /// Content hash of the script.
        /// </summary>
        [DataMember(Name = "hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Embedder-specific auxiliary data.
        /// </summary>
        [DataMember(Name = "executionContextAuxData")]
        public object executionContextAuxData { get; set; }
    }
}
