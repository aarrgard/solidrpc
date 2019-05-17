using System.Collections.Generic;

namespace SolidRpc.Tests.Swagger.Petstore
{
    public interface IPet
    {
        IEnumerable<Pet> FindPetsByStatus(IEnumerable<string> status);
    }
}
