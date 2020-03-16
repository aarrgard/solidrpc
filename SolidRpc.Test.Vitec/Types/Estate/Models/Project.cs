using System.CodeDom.Compiler;
using SolidRpc.Test.Vitec.Types.Common.Estate;
using System.Runtime.Serialization;
using System;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.CustomField.Models;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Project {
        /// <summary>
        /// Marknadsf&#246;ring
        /// </summary>
        [DataMember(Name="advertiseOn",EmitDefaultValue=false)]
        public AdvertiseOnProject AdvertiseOn { get; set; }
    
        /// <summary>
        /// Antal objekt associerade till projektet
        /// </summary>
        [DataMember(Name="numberOfObjects",EmitDefaultValue=false)]
        public int? NumberOfObjects { get; set; }
    
        /// <summary>
        /// S&#228;ljstartsdag
        /// </summary>
        [DataMember(Name="salesStartDate",EmitDefaultValue=false)]
        public DateTimeOffset? SalesStartDate { get; set; }
    
        /// <summary>
        /// Inflyttningsdatum
        /// </summary>
        [DataMember(Name="movingIn",EmitDefaultValue=false)]
        public string MovingIn { get; set; }
    
        /// <summary>
        /// Prisintervall
        /// </summary>
        [DataMember(Name="priceRange",EmitDefaultValue=false)]
        public Range PriceRange { get; set; }
    
        /// <summary>
        /// Storleksintervall p&#229; boarea
        /// </summary>
        [DataMember(Name="livingSpaceRange",EmitDefaultValue=false)]
        public Range LivingSpaceRange { get; set; }
    
        /// <summary>
        /// Intervall p&#229; antalrum
        /// </summary>
        [DataMember(Name="numberOfRoomsRange",EmitDefaultValue=false)]
        public Range NumberOfRoomsRange { get; set; }
    
        /// <summary>
        /// Basinformation ang&#229;ende projektet
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Estate.Models.ProjectBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Hus associerade till projektet
        /// </summary>
        [DataMember(Name="houses",EmitDefaultValue=false)]
        public IEnumerable<string> Houses { get; set; }
    
        /// <summary>
        /// L&#228;genheter som ing&#229;r i en bostadsr&#228;tts&#246;rening och &#228;r associerade till projektet
        /// </summary>
        [DataMember(Name="housingCooperatives",EmitDefaultValue=false)]
        public IEnumerable<string> HousingCooperatives { get; set; }
    
        /// <summary>
        /// Fritidsvillor som &#228;r associerade till projektet
        /// </summary>
        [DataMember(Name="cottages",EmitDefaultValue=false)]
        public IEnumerable<string> Cottages { get; set; }
    
        /// <summary>
        /// Tomter associerade till projektet
        /// </summary>
        [DataMember(Name="plots",EmitDefaultValue=false)]
        public IEnumerable<string> Plots { get; set; }
    
        /// <summary>
        /// Visningar p&#229; objekt som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="viewings",EmitDefaultValue=false)]
        public IEnumerable<ProjectViewing> Viewings { get; set; }
    
        /// <summary>
        /// N&#228;romr&#229;de
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public Surrounding Surrounding { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="condominiums",EmitDefaultValue=false)]
        public IEnumerable<string> Condominiums { get; set; }
    
        /// <summary>
        /// Lista &#246;ver aktuella k&#246;pare representerade av id:n
        /// </summary>
        [DataMember(Name="buyers",EmitDefaultValue=false)]
        public IEnumerable<string> Buyers { get; set; }
    
        /// <summary>
        /// Internetinst&#228;llningar
        /// </summary>
        [DataMember(Name="internetSettings",EmitDefaultValue=false)]
        public InternetSettings InternetSettings { get; set; }
    
        /// <summary>
        /// Kort bostadsid.
        /// </summary>
        [DataMember(Name="webbId",EmitDefaultValue=false)]
        public string WebbId { get; set; }
    
        /// <summary>
        /// Bostadsid
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Kontorsid
        /// </summary>
        [DataMember(Name="officeId",EmitDefaultValue=false)]
        public string OfficeId { get; set; }
    
        /// <summary>
        /// &#196;ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset? DateChanged { get; set; }
    
        /// <summary>
        /// Uppdraget
        /// </summary>
        [DataMember(Name="assignment",EmitDefaultValue=false)]
        public Assignment Assignment { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public Description Description { get; set; }
    
        /// <summary>
        /// Bilder
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<ObjectImage> Images { get; set; }
    
        /// <summary>
        /// Lista &#246;ver aktuella intressenter representerade av id:n
        /// </summary>
        [DataMember(Name="interests",EmitDefaultValue=false)]
        public IEnumerable<string> Interests { get; set; }
    
        /// <summary>
        /// Egendefinierade f&#228;lt
        /// </summary>
        [DataMember(Name="customFields",EmitDefaultValue=false)]
        public IEnumerable<FieldValue> CustomFields { get; set; }
    
    }
}