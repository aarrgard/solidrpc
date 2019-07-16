using SolidRpc.Test.Petstore.Services;
using SolidRpc.Test.Petstore.Types;
using System;
using System.Collections.Generic;
using System.IO;

using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.Impl
{
    public class PetImpl : IPet
    {
        public Task AddPet(Pet body, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeletePet(string api_key, long petId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pet>> FindPetsByStatus(IEnumerable<string> status, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pet>> FindPetsByTags(IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Pet> GetPetById(long petId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task UpdatePet(Pet body, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task UpdatePetWithForm(long petId, string name, string status, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UploadFile(long petId, string additionalMetadata, Stream file, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
