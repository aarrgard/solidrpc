using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicAdvertisingEstateList {
        /// <summary>
        /// Lista p� villor
        /// </summary>
        [DataMember(Name="houses",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Houses { get; set; }
    
        /// <summary>
        /// Lista p� fritidsvillor
        /// </summary>
        [DataMember(Name="cottages",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Cottages { get; set; }
    
        /// <summary>
        /// Lista p� bostadsr�tter
        /// </summary>
        [DataMember(Name="housingCooperatives",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> HousingCooperatives { get; set; }
    
        /// <summary>
        /// Lista p� tomter
        /// </summary>
        [DataMember(Name="plots",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Plots { get; set; }
    
        /// <summary>
        /// Lista p� g�rdar
        /// </summary>
        [DataMember(Name="farms",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Farms { get; set; }
    
        /// <summary>
        /// Lista p� kommersiella objekt
        /// </summary>
        [DataMember(Name="commercialProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> CommercialProperties { get; set; }
    
        /// <summary>
        /// Lista p� �garl�genheter
        /// </summary>
        [DataMember(Name="condominiums",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Condominiums { get; set; }
    
        /// <summary>
        /// Lista p� utlandsbost�der
        /// </summary>
        [DataMember(Name="foreignProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> ForeignProperties { get; set; }
    
        /// <summary>
        /// Lista p� lokaler
        /// </summary>
        [DataMember(Name="premises",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Premises { get; set; }
    
        /// <summary>
        /// Lista p� projekt
        /// </summary>
        [DataMember(Name="projects",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Projects { get; set; }
    
    }
}