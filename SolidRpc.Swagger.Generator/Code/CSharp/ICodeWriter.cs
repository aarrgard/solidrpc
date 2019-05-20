using System.IO;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// The code writer writes code to a folder or zip
    /// </summary>
    public interface ICodeWriter
    {
        string NewLine { get; }

        /// <summary>
        /// Moves to supplied file name.
        /// </summary>
        /// <param name="fileName"></param>
        void MoveToFile(string fileName);
        void Emit(string v);
        void Indent();
        void Unindent();
    }
}