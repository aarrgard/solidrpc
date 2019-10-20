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
        /// Marknadsf�ring
        /// </summary>
        [DataMember(Name="advertiseOn",EmitDefaultValue=false)]
        public AdvertiseOnProject AdvertiseOn { get; set; }
    
        /// <summary>
        /// Antal objekt associerade till projektet
        /// </summary>
        [DataMember(Name="numberOfObjects",EmitDefaultValue=false)]
        public int NumberOfObjects { get; set; }
    
        /// <summary>
        /// S�ljstartsdag
        /// </summary>
        [DataMember(Name="salesStartDate",EmitDefaultValue=false)]
        public DateTimeOffset SalesStartDate { get; set; }
    
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
        /// Storleksintervall p� boarea
        /// </summary>
        [DataMember(Name="livingSpaceRange",EmitDefaultValue=false)]
        public Range LivingSpaceRange { get; set; }
    
        /// <summary>
        /// Intervall p� antalrum
        /// </summary>
        [DataMember(Name="numberOfRoomsRange",EmitDefaultValue=false)]
        public Range NumberOfRoomsRange { get; set; }
    
        /// <summary>
        /// Basinformation ang�ende projektet
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Estate.Models.ProjectBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Hus associerade till projektet
        /// </summary>
        [DataMember(Name="houses",EmitDefaultValue=false)]
        public IEnumerable<string> Houses { get; set; }
    
        /// <summary>
        /// L�genheter som ing�r i en bostadsr�tts�rening och �r associerade till projektet
        /// </summary>
        [DataMember(Name="housingCooperatives",EmitDefaultValue=false)]
        public IEnumerable<string> HousingCooperatives { get; set; }
    
        /// <summary>
        /// Fritidsvillor som �r associerade till projektet
        /// </summary>
        [DataMember(Name="cottages",EmitDefaultValue=false)]
        public IEnumerable<string> Cottages { get; set; }
    
        /// <summary>
        /// Tomter associerade till projektet
        /// </summary>
        [DataMember(Name="plots",EmitDefaultValue=false)]
        public IEnumerable<string> Plots { get; set; }
    
        /// <summary>
        /// Visningar p� objekt som ing�r i projektet
        /// </summary>
        [DataMember(Name="viewings",EmitDefaultValue=false)]
        public IEnumerable<ProjectViewing> Viewings { get; set; }
    
        /// <summary>
        /// N�romr�de
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public Surrounding Surrounding { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="condominiums",EmitDefaultValue=false)]
        public IEnumerable<string> Condominiums { get; set; }
    
        /// <summary>
        /// Lista �ver aktuella k�pare representerade av id:n
        /// </summary>
        [DataMember(Name="buyers",EmitDefaultValue=false)]
        public IEnumerable<string> Buyers { get; set; }
    
        /// <summary>
        /// Internetinst�llningar
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
        /// �ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset DateChanged { get; set; }
    
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
        /// Lista �ver aktuella intressenter representerade av id:n
        /// </summary>
        [DataMember(Name="interests",EmitDefaultValue=false)]
        public IEnumerable<string> Interests { get; set; }
    
        /// <summary>
        /// Egendefinierade f�lt
        /// </summary>
        [DataMember(Name="customFields",EmitDefaultValue=false)]
        public IEnumerable<FieldValue> CustomFields { get; set; }
    
    }
}