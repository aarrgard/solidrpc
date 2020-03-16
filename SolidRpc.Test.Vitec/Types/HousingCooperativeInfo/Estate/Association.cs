using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.HousingCooperativeInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Association {
        /// <summary>
        /// Id p&#229; f&#246;reningen
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// F&#246;reningsnamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Organisationsform
        /// </summary>
        [DataMember(Name="organizationalForm",EmitDefaultValue=false)]
        public string OrganizationalForm { get; set; }
    
        /// <summary>
        /// Organisationsnummer
        /// </summary>
        [DataMember(Name="corporateNumber",EmitDefaultValue=false)]
        public string CorporateNumber { get; set; }
    
        /// <summary>
        /// Gatuadress
        /// </summary>
        [DataMember(Name="streetAddress",EmitDefaultValue=false)]
        public string StreetAddress { get; set; }
    
        /// <summary>
        /// Postnummer
        /// </summary>
        [DataMember(Name="zipCode",EmitDefaultValue=false)]
        public string ZipCode { get; set; }
    
        /// <summary>
        /// Ort
        /// </summary>
        [DataMember(Name="city",EmitDefaultValue=false)]
        public string City { get; set; }
    
        /// <summary>
        /// Epostadress
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public Email Email { get; set; }
    
        /// <summary>
        /// Hemsida
        /// </summary>
        [DataMember(Name="homePage",EmitDefaultValue=false)]
        public string HomePage { get; set; }
    
        /// <summary>
        /// &#196;kta/o&#228;kta f&#246;rening
        /// </summary>
        [DataMember(Name="genuineAssociation",EmitDefaultValue=false)]
        public bool? GenuineAssociation { get; set; }
    
        /// <summary>
        /// Kontaktperson (offentlig)
        /// </summary>
        [DataMember(Name="publicContact",EmitDefaultValue=false)]
        public string PublicContact { get; set; }
    
        /// <summary>
        /// Telefon Kontaktperson
        /// </summary>
        [DataMember(Name="phonePublicContact",EmitDefaultValue=false)]
        public string PhonePublicContact { get; set; }
    
        /// <summary>
        /// Ans&#246;kan in/uttr&#228;de
        /// </summary>
        [DataMember(Name="applicationEntranceResignation",EmitDefaultValue=false)]
        public string ApplicationEntranceResignation { get; set; }
    
        /// <summary>
        /// Ansvarig l&#228;genhetsregister
        /// </summary>
        [DataMember(Name="responsibleApartmentRegister",EmitDefaultValue=false)]
        public string ResponsibleApartmentRegister { get; set; }
    
        /// <summary>
        /// Allm&#228;nt om f&#246;reningen
        /// </summary>
        [DataMember(Name="generalAboutAssociation",EmitDefaultValue=false)]
        public string GeneralAboutAssociation { get; set; }
    
        /// <summary>
        /// Anteckning p&#229; aktuell bostadsr&#228;tt
        /// </summary>
        [DataMember(Name="noteAboutHousingCooperative",EmitDefaultValue=false)]
        public string NoteAboutHousingCooperative { get; set; }
    
        /// <summary>
        /// Anteckningar(ej i annonser)
        /// </summary>
        [DataMember(Name="notes",EmitDefaultValue=false)]
        public string Notes { get; set; }
    
        /// <summary>
        /// F&#246;reningens ekonomi
        /// </summary>
        [DataMember(Name="finances",EmitDefaultValue=false)]
        public string Finances { get; set; }
    
        /// <summary>
        /// &#196;ger f&#246;reningen marken
        /// </summary>
        [DataMember(Name="theAssociationOwnTheGround",EmitDefaultValue=false)]
        public string TheAssociationOwnTheGround { get; set; }
    
        /// <summary>
        /// Antal l&#228;genheter
        /// </summary>
        [DataMember(Name="numberOfApartments",EmitDefaultValue=false)]
        public int? NumberOfApartments { get; set; }
    
        /// <summary>
        /// Antal hyresr&#228;tter
        /// </summary>
        [DataMember(Name="numberOfRentalUnits",EmitDefaultValue=false)]
        public int? NumberOfRentalUnits { get; set; }
    
        /// <summary>
        /// Antal lokaler
        /// </summary>
        [DataMember(Name="numberOfLocals",EmitDefaultValue=false)]
        public int? NumberOfLocals { get; set; }
    
        /// <summary>
        /// Renoveringar, utf&#246;rda och planerade
        /// </summary>
        [DataMember(Name="renovations",EmitDefaultValue=false)]
        public string Renovations { get; set; }
    
        /// <summary>
        /// Parkering
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public string Parking { get; set; }
    
        /// <summary>
        /// Tv och bredband
        /// </summary>
        [DataMember(Name="tvAndBroadband",EmitDefaultValue=false)]
        public string TvAndBroadband { get; set; }
    
        /// <summary>
        /// &#214;verl&#229;telseavgift
        /// </summary>
        [DataMember(Name="transferFee",EmitDefaultValue=false)]
        public double? TransferFee { get; set; }
    
        /// <summary>
        /// Pants&#228;ttningsavgift
        /// </summary>
        [DataMember(Name="pledgeFee",EmitDefaultValue=false)]
        public double? PledgeFee { get; set; }
    
        /// <summary>
        /// F&#246;reningen till&#229;ter juridisk person som k&#246;pare
        /// </summary>
        [DataMember(Name="allowLegalPersonAsBuyer",EmitDefaultValue=false)]
        public string AllowLegalPersonAsBuyer { get; set; }
    
        /// <summary>
        /// F&#246;reningen till&#229;ter dubbelt &#228;gande
        /// </summary>
        [DataMember(Name="allowTwinOwnership",EmitDefaultValue=false)]
        public string AllowTwinOwnership { get; set; }
    
        /// <summary>
        /// &#214;verl&#229;telseavgift betalas av
        /// </summary>
        [DataMember(Name="transferFeePaidBy",EmitDefaultValue=false)]
        public string TransferFeePaidBy { get; set; }
    
        /// <summary>
        /// G&#229;rdsplats
        /// </summary>
        [DataMember(Name="courtyard",EmitDefaultValue=false)]
        public string Courtyard { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Gemensamma utrymmen
        /// </summary>
        [DataMember(Name="sharedSpaces",EmitDefaultValue=false)]
        public string SharedSpaces { get; set; }
    
        /// <summary>
        /// Publika dokument
        /// </summary>
        [DataMember(Name="documents",EmitDefaultValue=false)]
        public IEnumerable<Document> Documents { get; set; }
    
        /// <summary>
        /// M&#229;nadsavgifts kommentar
        /// </summary>
        [DataMember(Name="monthlyFeeinfinformation",EmitDefaultValue=false)]
        public string MonthlyFeeinfinformation { get; set; }
    
        /// <summary>
        /// Bilder
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<AssociationImage> Images { get; set; }
    
        /// <summary>
        /// F&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public string Insurance { get; set; }
    
        /// <summary>
        /// Bygg&#229;r
        /// </summary>
        [DataMember(Name="buildingYear",EmitDefaultValue=false)]
        public string BuildingYear { get; set; }
    
        /// <summary>
        /// CommentaryForBuildingYear
        /// </summary>
        [DataMember(Name="commentaryForBuildingYear",EmitDefaultValue=false)]
        public string CommentaryForBuildingYear { get; set; }
    
    }
}