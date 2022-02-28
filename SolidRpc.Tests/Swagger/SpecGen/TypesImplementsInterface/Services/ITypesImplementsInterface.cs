
using SolidRpc.Tests.Swagger.SpecGen.TypesImplementsInterface.Types.Sub;
using System;

namespace SolidRpc.Tests.Swagger.SpecGen.TypesImplementsInterface.Services
{
    /// <summary>
    /// Tests method with one complex type
    /// </summary>
    public interface ITypesImplementsInterface
    {
        ComplexType GetData();
    }
}
