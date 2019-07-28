namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    public class CSharpConstructor : CSharpMember, ICSharpConstructor
    {

        public CSharpConstructor(ICSharpClass parent, string baseArgs = null, string code = null) : base(parent, "<ctr>")
        {
            BaseArgs = baseArgs;
            Code = code;
        }

        public string BaseArgs { get; }
        public string Code { get; }

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