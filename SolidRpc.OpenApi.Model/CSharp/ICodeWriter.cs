namespace SolidRpc.OpenApi.Model.CSharp
{
    public interface ICodeWriter
    {
        /// <summary>
        /// The representation of a newline
        /// </summary>
        string NewLine { get; }

        /// <summary>
        /// Moves to supplied file name.
        /// </summary>
        /// <param name="fileName"></param>
        void MoveToClassFile(string fileName);

        /// <summary>
        /// Emits the supplied string to the current file.
        /// </summary>
        /// <param name="str"></param>
        void Emit(string str);

        /// <summary>
        /// Indents 
        /// </summary>
        void Indent();

        /// <summary>
        /// Unindent
        /// </summary>
        void Unindent();
    }
}