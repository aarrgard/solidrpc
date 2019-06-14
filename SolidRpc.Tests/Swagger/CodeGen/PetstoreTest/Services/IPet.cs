using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.PetstoreTest.Types;
using System.Threading;
using System.Collections.Generic;
using System.IO;
namespace SolidRpc.Tests.Swagger.CodeGen.PetstoreTest.Services {
    /// <summary>
    /// Everything about your Pets
    /// </summary>
    public interface IPet {
        /// <summary>
        /// Add a new pet to the store
        /// </summary>
        /// <param name="body">Pet object that needs to be added to the store</param>
        /// <param name="cancellationToken"></param>
        Task AddPet(
            Pet body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Update an existing pet
        /// </summary>
        /// <param name="body">Pet object that needs to be added to the store</param>
        /// <param name="cancellationToken"></param>
        Task UpdatePet(
            Pet body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Finds Pets by status Multiple status values can be provided with comma separated strings
        /// </summary>
        /// <param name="status">Status values that need to be considered for filter</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Pet>> FindPetsByStatus(
            IEnumerable<string> status,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Finds Pets by tags Muliple tags can be provided with comma separated strings. Use\ \ tag1, tag2, tag3 for testing.
        /// </summary>
        /// <param name="tags">Tags to filter by</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Pet>> FindPetsByTags(
            IEnumerable<string> tags,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Deletes a pet
        /// </summary>
        /// <param name="api_key"></param>
        /// <param name="petId">Pet id to delete</param>
        /// <param name="cancellationToken"></param>
        Task DeletePet(
            string api_key,
            long petId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Find pet by ID Returns a single pet
        /// </summary>
        /// <param name="petId">ID of pet to return</param>
        /// <param name="cancellationToken"></param>
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
        Task UpdatePetWithForm(
            long petId,
            string name,
            string status,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// uploads an image
        /// </summary>
        /// <param name="petId">ID of pet to update</param>
        /// <param name="additionalMetadata">Additional data to pass to server</param>
        /// <param name="file">file to upload</param>
        /// <param name="cancellationToken"></param>
        Task<ApiResponse> UploadFile(
            long petId,
            string additionalMetadata,
            Stream file,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}