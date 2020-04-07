using System.IO;

namespace SolidRpc.Tests.Swagger.SpecGen.Redirect.Types
{
    /// <summary>
    /// The redirect
    /// </summary>
    public class Redirect
    {
        /// <summary>
        /// The content
        /// </summary>
        public Stream Content { get; set; }

        /// <summary>
        /// Where should we redirect.
        /// </summary>
        public string Location { get; set; }
    }
}
