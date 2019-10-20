namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Implements a C# constructor
    /// </summary>
    public class CSharpConstructor : CSharpMember, ICSharpConstructor
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="baseArgs"></param>
        /// <param name="code"></param>
        public CSharpConstructor(ICSharpClass parent, string baseArgs = null, string code = null) : base(parent, "<ctr>")
        {
            BaseArgs = baseArgs;
            Code = code;
        }

        /// <summary>
        /// The base arguments
        /// </summary>
        public string BaseArgs { get; }

        /// <summary>
        /// The constructor code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Emits the constructor to supplied writer.
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            var baseInit = "";
            if(!string.IsNullOrEmpty(BaseArgs))
            {
                baseInit = $" : base({BaseArgs})";
            }
            WriteSummary(codeWriter);
            codeWriter.Emit($"public {Parent.Name}(){baseInit}{codeWriter.NewLine}{{");
            if(!string.IsNullOrEmpty(Code))
            {
                codeWriter.Indent();
                codeWriter.Emit($"{codeWriter.NewLine}{Code}{codeWriter.NewLine}");
                codeWriter.Unindent();
            }
            codeWriter.Emit($"}}");
        }
    }
}