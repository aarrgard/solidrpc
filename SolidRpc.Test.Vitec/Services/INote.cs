using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Models = SolidRpc.Test.Vitec.Types.Note.Models;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface INote {
        /// <summary>
        /// Sparar en anteckning
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="note"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Note.NoteSaveNote.NoContentException">No Content</exception>
        Task NoteSaveNote(
            string customerId,
            Models.Note note,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar lista av anteckningar f�r en kontakt
        /// </summary>
        /// <param name="contactId">kontaktId</param>
        /// <param name="customerId">Kund-id</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.SimpleNote>> NoteGetNotes(
            string contactId,
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}