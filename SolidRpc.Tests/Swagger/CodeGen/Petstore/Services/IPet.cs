using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Security;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Types;
using System.Threading;
using System.Collections.Generic;
using System.IO;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Services {
    /// <summary>
    /// Everything about your Pets
    /// </summary>
    /// <a href="http://swagger.io">Find out more</a>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPet {
        /// <summary>
        /// Add a new pet to the store
        /// </summary>
        /// <param name="body">Pet object that needs to be added to the store</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.AddPet.InvalidInputException">Invalid input</exception>
        [PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
        Task AddPet(
            Pet body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Update an existing pet
        /// </summary>
        /// <param name="body">Pet object that needs to be added to the store</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.UpdatePet.InvalidIDSuppliedException">Invalid ID supplied</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.UpdatePet.PetNotFoundException">Pet not found</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.UpdatePet.ValidationException">Validation exception</exception>
        [PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
        Task UpdatePet(
            Pet body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Finds Pets by status Multiple status values can be provided with comma separated strings
        /// </summary>
        /// <param name="status">Status values that need to be considered for filter</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.FindPetsByStatus.InvalidStatusValueException">Invalid status value</exception>
        [PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
        Task<IEnumerable<Pet>> FindPetsByStatus(
            IEnumerable<string> status,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Finds Pets by tags Muliple tags can be provided with comma separated strings. Use\ \ tag1, tag2, tag3 for testing.
        /// </summary>
        /// <param name="tags">Tags to filter by</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.FindPetsByTags.InvalidTagValueException">Invalid tag value</exception>
        [PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
        Task<IEnumerable<Pet>> FindPetsByTags(
            IEnumerable<string> tags,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Deletes a pet
        /// </summary>
        /// <param name="petId">Pet id to delete</param>
        /// <param name="apiKey"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.DeletePet.InvalidIDSuppliedException">Invalid ID supplied</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.DeletePet.PetNotFoundException">Pet not found</exception>
        [PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
        Task DeletePet(
            long petId,
            string apiKey = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Find pet by ID Returns a single pet
        /// </summary>
        /// <param name="petId">ID of pet to return</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.GetPetById.InvalidIDSuppliedException">Invalid ID supplied</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.GetPetById.PetNotFoundException">Pet not found</exception>
        [ApiKey]
        Task<Pet> GetPetById(
            long petId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Updates a pet in the store with form data
        /// </summary>
        /// <param name="petId">ID of pet that needs to be updated</param>
        /// <param name="name">Updated name of the pet</param>
        /// <param name="status">Updated status of the pet</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Pet.UpdatePetWithForm.InvalidInputException">Invalid input</exception>
        [PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
        Task UpdatePetWithForm(
            long petId,
            string name = null,
            string status = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// uploads an image
        /// </summary>
        /// <param name="petId">ID of pet to update</param>
        /// <param name="additionalMetadata">Additional data to pass to server</param>
        /// <param name="file">file to upload</param>
        /// <param name="cancellationToken"></param>
        [PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
        Task<ApiResponse> UploadFile(
            long petId,
            string additionalMetadata = null,
            Stream file = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}