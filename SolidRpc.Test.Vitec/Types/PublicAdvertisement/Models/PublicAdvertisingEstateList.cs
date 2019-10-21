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
        /// Lista p&#229; villor
        /// </summary>
        [DataMember(Name="houses",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Houses { get; set; }
    
        /// <summary>
        /// Lista p&#229; fritidsvillor
        /// </summary>
        [DataMember(Name="cottages",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Cottages { get; set; }
    
        /// <summary>
        /// Lista p&#229; bostadsr&#228;tter
        /// </summary>
        [DataMember(Name="housingCooperatives",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> HousingCooperatives { get; set; }
    
        /// <summary>
        /// Lista p&#229; tomter
        /// </summary>
        [DataMember(Name="plots",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Plots { get; set; }
    
        /// <summary>
        /// Lista p&#229; g&#229;rdar
        /// </summary>
        [DataMember(Name="farms",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Farms { get; set; }
    
        /// <summary>
        /// Lista p&#229; kommersiella objekt
        /// </summary>
        [DataMember(Name="commercialProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> CommercialProperties { get; set; }
    
        /// <summary>
        /// Lista p&#229; &#228;garl&#228;genheter
        /// </summary>
        [DataMember(Name="condominiums",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Condominiums { get; set; }
    
        /// <summary>
        /// Lista p&#229; utlandsbost&#228;der
        /// </summary>
        [DataMember(Name="foreignProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> ForeignProperties { get; set; }
    
        /// <summary>
        /// Lista p&#229; lokaler
        /// </summary>
        [DataMember(Name="premises",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Premises { get; set; }
    
        /// <summary>
        /// Lista p&#229; projekt
        /// </summary>
        [DataMember(Name="projects",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateInList> Projects { get; set; }
    
    }
}