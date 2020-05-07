using SolidRpc.Tests.Swagger.SpecGen.TwoComplexArgs.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.SameMethodNameNbrArgs.Services
{
    /// <summary>
    /// Tests method with two complex types
    /// </summary>
    public interface ISameMethodNameNbrArgs
    {
        /// <summary>
        /// consumes one string
        /// </summary>
        /// <param name="s1"></param>
        /// <returns></returns>
        void ConsumeString(string s1);

        /// <summary>
        /// consumes two strings
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        void ConsumeString(string s1, string s2);
    }
}
