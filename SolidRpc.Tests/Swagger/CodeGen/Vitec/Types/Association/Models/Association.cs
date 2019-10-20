using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Association.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Association {
        /// <summary>
        /// F�reningsnamn
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
        /// �kta/o�kta f�rening
        /// </summary>
        [DataMember(Name="genuineAssociation",EmitDefaultValue=false)]
        public bool GenuineAssociation { get; set; }
    
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
        /// Ans�kan in/uttr�de
        /// </summary>
        [DataMember(Name="applicationEntranceResignation",EmitDefaultValue=false)]
        public string ApplicationEntranceResignation { get; set; }
    
        /// <summary>
        /// Ansvarig l�genhetsregister
        /// </summary>
        [DataMember(Name="responsibleApartmentRegister",EmitDefaultValue=false)]
        public string ResponsibleApartmentRegister { get; set; }
    
        /// <summary>
        /// Allm�nt om f�reningen
        /// </summary>
        [DataMember(Name="generalAboutAssociation",EmitDefaultValue=false)]
        public string GeneralAboutAssociation { get; set; }
    
        /// <summary>
        /// Anteckningar(ej i annonser)
        /// </summary>
        [DataMember(Name="notes",EmitDefaultValue=false)]
        public string Notes { get; set; }
    
        /// <summary>
        /// F�reningens ekonomi
        /// </summary>
        [DataMember(Name="finances",EmitDefaultValue=false)]
        public string Finances { get; set; }
    
        /// <summary>
        /// �ger f�reningen marken
        /// </summary>
        [DataMember(Name="theAssociationOwnTheGround",EmitDefaultValue=false)]
        public string TheAssociationOwnTheGround { get; set; }
    
        /// <summary>
        /// Antal l�genheter
        /// </summary>
        [DataMember(Name="numberOfApartments",EmitDefaultValue=false)]
        public int NumberOfApartments { get; set; }
    
        /// <summary>
        /// Antal hyresr�tter
        /// </summary>
        [DataMember(Name="numberOfRentalUnits",EmitDefaultValue=false)]
        public int NumberOfRentalUnits { get; set; }
    
        /// <summary>
        /// Antal lokaler
        /// </summary>
        [DataMember(Name="numberOfLocals",EmitDefaultValue=false)]
        public int NumberOfLocals { get; set; }
    
        /// <summary>
        /// Renoveringar, utf�rda och planerade
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
        /// �verl�telseavgift
        /// </summary>
        [DataMember(Name="transferFee",EmitDefaultValue=false)]
        public double TransferFee { get; set; }
    
        /// <summary>
        /// Pants�ttningsavgift
        /// </summary>
        [DataMember(Name="pledgeFee",EmitDefaultValue=false)]
        public double PledgeFee { get; set; }
    
        /// <summary>
        /// F�reningen till�ter juridisk person som k�pare
        /// </summary>
        [DataMember(Name="allowLegalPersonAsBuyer",EmitDefaultValue=false)]
        public string AllowLegalPersonAsBuyer { get; set; }
    
        /// <summary>
        /// F�reningen till�ter dubbelt �gande
        /// </summary>
        [DataMember(Name="allowTwinOwnership",EmitDefaultValue=false)]
        public string AllowTwinOwnership { get; set; }
    
        /// <summary>
        /// �verl�telseavgift betalas av
        /// </summary>
        [DataMember(Name="transferFeePaidBy",EmitDefaultValue=false)]
        public string TransferFeePaidBy { get; set; }
    
        /// <summary>
        /// G�rdsplats
        /// </summary>
        [DataMember(Name="courtyard",EmitDefaultValue=false)]
        public string Courtyard { get; set; }
    
        /// <summary>
        /// �vrigt
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
        /// M�nadsavgifts kommentar
        /// </summary>
        [DataMember(Name="monthlyFeeinfinformation",EmitDefaultValue=false)]
        public string MonthlyFeeinfinformation { get; set; }
    
        /// <summary>
        /// Bilder
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<AssociationImage> Images { get; set; }
    
        /// <summary>
        /// F�rs�kring
        /// </summary>
        [DataMember(Name="insurance",EmitDefaultValue=false)]
        public string Insurance { get; set; }
    
        /// <summary>
        /// Bygg�r
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