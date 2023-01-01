using System;

namespace SolidRpc.Tests.Swagger.SpecGen.Exceptions.Types
{
    /// <summary>
    /// The test exception
    /// </summary>
    public class TestException : Exception
    {

        /// <summary>
        /// The http status code
        /// </summary>
        public static readonly int HttpStatusCode = 412;

        private void Init()
        {
            Data["HttpStatusCode"] = HttpStatusCode;
        }

        /// <summary>
        /// Constructs a new exception
        /// </summary>
        public TestException()
        {
            Init();
        }
    }
}
