using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingImage {
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
        Task<Stream> PublicAdvertisingImageGet(
            string customerId,
            string imageId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}