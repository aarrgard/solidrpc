using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Premises {
        /// <summary>
        /// Lokalnummer
        /// </summary>
        [DataMember(Name="number",EmitDefaultValue=false)]
        public string Number { get; set; }
    
        /// <summary>
        /// Fastighetsbeteckning
        /// </summary>
        [DataMember(Name="propertyDesignation",EmitDefaultValue=false)]
        public string PropertyDesignation { get; set; }
    
        /// <summary>
        /// Projektid om lokalen ing�r i ett projekt
        /// </summary>
        [DataMember(Name="projectId",EmitDefaultValue=false)]
        public string ProjectId { get; set; }
    
        /// <summary>
        /// Marknadsf�ring
        /// </summary>
        [DataMember(Name="marketing",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.PremisesMarketing Marketing { get; set; }
    
        /// <summary>
        /// Byggnad
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.PremisesBuilding Building { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Utgifter
        /// </summary>
        [DataMember(Name="expenses",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.PremisesExpenses Expenses { get; set; }
    
        /// <summary>
        /// Prisuppgift
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Price Price { get; set; }
    
        /// <summary>
        /// Ytor
        /// </summary>
        [DataMember(Name="surfaces",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.PremisesSurfaces Surfaces { get; set; }
    
        /// <summary>
        /// Ytor lista
        /// </summary>
        [DataMember(Name="surfaceList",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.PremisesSurface> SurfaceList { get; set; }
    
        /// <summary>
        /// Typ av lokal
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Bostadens id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Kontorstillh�righet
        /// </summary>
        [DataMember(Name="officeAffiliation",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.OfficeAffiliation OfficeAffiliation { get; set; }
    
        /// <summary>
        /// Ytterligare specialiseringsattribut
        /// </summary>
        [DataMember(Name="customAttributes",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomAttributes CustomAttributes { get; set; }
    
        /// <summary>
        /// Id p� huvudhandl�ggare
        /// </summary>
        [DataMember(Name="primaryAgentId",EmitDefaultValue=false)]
        public string PrimaryAgentId { get; set; }
    
        /// <summary>
        /// Id p� andrahandl�ggare
        /// </summary>
        [DataMember(Name="secondaryAgentId",EmitDefaultValue=false)]
        public string SecondaryAgentId { get; set; }
    
        /// <summary>
        /// Adress och geografiska uppgifter
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Address Address { get; set; }
    
        /// <summary>
        /// Texter
        /// </summary>
        [DataMember(Name="texts",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Texts Texts { get; set; }
    
        /// <summary>
        /// N�romr�de
        /// </summary>
        [DataMember(Name="surroundings",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Surroundings Surroundings { get; set; }
    
        /// <summary>
        /// Om budgivning p�g�r
        /// </summary>
        [DataMember(Name="acceptingBids",EmitDefaultValue=false)]
        public bool AcceptingBids { get; set; }
    
        /// <summary>
        /// N�r bostaden senast �ndrades
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAt { get; set; }
    
        /// <summary>
        /// Url:er
        /// </summary>
        [DataMember(Name="urls",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Urls Urls { get; set; }
    
        /// <summary>
        /// Visningar
        /// </summary>
        [DataMember(Name="viewings",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Viewing> Viewings { get; set; }
    
        /// <summary>
        /// Bilder
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Image> Images { get; set; }
    
    }
}