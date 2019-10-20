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
    public interface Image {
        /// <summary>
        /// H�mtar bilder. &lt;p&gt;H�mtar bild.&lt;/p&gt;
        /// &lt;p&gt;F�r att kunna h�mta en bild s� kr�vs det en giltig API nyckel och ett kundid.
        /// Det kr�vs �ven ett giltig bild id f�r att kunna h�mta en bild.&lt;/p&gt;
        /// &lt;p&gt;
        /// &lt;a href=&quot;/Help/ImageApi&quot; target=&quot;_blank&quot;&gt;Det finns dessutom extra parametrar f�r t ex skalning och besk�rning av bilden,
        /// mer information finns h�r&lt;/a&gt;
        /// &lt;/p&gt;
        /// &lt;p&gt;OBS! Om inga parametrar f�r bild API:et anv�nds, s� kommer ni att f� bilden exakt som bilden ligger lagrad i datak�llan.&lt;/p&gt;
        /// </summary>
        /// <a href="/Help/ImageApi">Det finns dessutom extra parametrar f�r t ex skalning och besk�rning av bilden,
        /// mer information finns h�r</a>
        /// <param name="customerId">Kundid</param>
        /// <param name="imageId">Bildid</param>
        /// <param name="cancellationToken"></param>
        Task<Stream> ImageGetImage(
            string customerId,
            string imageId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att l�gga till en ny bild (gif,jpg,tiff,bmp,png) till en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="imageData">Bilddata</param>
        /// <param name="cancellationToken"></param>
        Task<string> ImagePost(
            string customerId,
            string estateId,
            ImageData imageData,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att uppdatera en bild (gif,jpg,tiff,bmp,png).
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="imageId">Bildid</param>
        /// <param name="imageData">Bilddata</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Image.ImagePut.NoContentException">No Content</exception>
        Task ImagePut(
            string customerId,
            string imageId,
            ImageData imageData,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att h�mta bildkategorier
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<ImageCategories>> ImageGetImageCategories(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}