using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using SolidRpc.Test.Vitec.Types.Image.Models;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmImage {
        /// <summary>
        /// H&#228;mtar bilder. &lt;p&gt;H&#228;mtar bild.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta en bild s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// Det kr&#228;vs &#228;ven ett giltig bild id f&#246;r att kunna h&#228;mta en bild.&lt;/p&gt;
        /// &lt;p&gt;
        /// &lt;a href=&quot;/Help/ImageApi&quot; target=&quot;_blank&quot;&gt;Det finns dessutom extra parametrar f&#246;r t ex skalning och besk&#228;rning av bilden,
        /// mer information finns h&#228;r&lt;/a&gt;
        /// &lt;/p&gt;
        /// &lt;p&gt;OBS! Om inga parametrar f&#246;r bild API:et anv&#228;nds, s&#229; kommer ni att f&#229; bilden exakt som bilden ligger lagrad i datak&#228;llan.&lt;/p&gt;
        /// </summary>
        /// <a href="/Help/ImageApi">Det finns dessutom extra parametrar för t ex skalning och beskärning av bilden,
        /// mer information finns här</a>
        /// <param name="customerId">Kundid</param>
        /// <param name="imageId">Bildid</param>
        /// <param name="cancellationToken"></param>
        Task<Stream> CrmImageGetImage(
            string customerId,
            string imageId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att l&#228;gga till en ny bild (gif,jpg,tiff,bmp,png) till en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="imageData">Bilddata</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmImagePost(
            string customerId,
            string estateId,
            ImageData imageData,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att ta bort en bild
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="imageId">Bildid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmImage.CrmImageDelete.NoContentException">No Content</exception>
        Task CrmImageDelete(
            string customerId,
            string imageId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att uppdatera en bild (gif,jpg,tiff,bmp,png).
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="imageId">Bildid</param>
        /// <param name="imageData">Bilddata</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmImage.CrmImagePut.NoContentException">No Content</exception>
        Task CrmImagePut(
            string customerId,
            string imageId,
            ImageData imageData,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att h&#228;mta bildkategorier
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<ImageCategories>> CrmImageGetImageCategories(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}