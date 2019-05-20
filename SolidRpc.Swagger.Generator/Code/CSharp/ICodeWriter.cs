using System.IO;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// The code writer writes code to a folder or zip
    /// </summary>
    public interface ICodeWriter
    {
        /// <summary>
        /// Returns the text writer that can be used to write data to supplied filename.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        TextWriter GetTextWriter(string fileName);
    }
}