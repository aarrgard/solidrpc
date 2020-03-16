using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CompilationAreas {
        /// <summary>
        /// Areak&#228;lla
        /// </summary>
        [DataMember(Name="areaSource",EmitDefaultValue=false)]
        public string AreaSource { get; set; }
    
        /// <summary>
        /// Bost&#228;der
        /// </summary>
        [DataMember(Name="residential",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Residential { get; set; }
    
        /// <summary>
        /// Garage
        /// </summary>
        [DataMember(Name="garage",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Garage { get; set; }
    
        /// <summary>
        /// P-plats
        /// </summary>
        [DataMember(Name="parkingLot",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea ParkingLot { get; set; }
    
        /// <summary>
        /// Kontor
        /// </summary>
        [DataMember(Name="office",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Office { get; set; }
    
        /// <summary>
        /// Butiker
        /// </summary>
        [DataMember(Name="stores",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Stores { get; set; }
    
        /// <summary>
        /// Industri/verkstad
        /// </summary>
        [DataMember(Name="industry",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Industry { get; set; }
    
        /// <summary>
        /// Varmlager
        /// </summary>
        [DataMember(Name="warmStockroom",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea WarmStockroom { get; set; }
    
        /// <summary>
        /// Kallager
        /// </summary>
        [DataMember(Name="coldStockroom",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea ColdStockroom { get; set; }
    
        /// <summary>
        /// &#214;vrigt 1
        /// </summary>
        [DataMember(Name="other1",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationAreaOther Other1 { get; set; }
    
        /// <summary>
        /// &#214;vrigt 2
        /// </summary>
        [DataMember(Name="other2",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationAreaOther Other2 { get; set; }
    
        /// <summary>
        /// &#214;vriga areor
        /// </summary>
        [DataMember(Name="others",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CompilationAreaOther> Others { get; set; }
    
        /// <summary>
        /// Areor
        /// </summary>
        [DataMember(Name="areas",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.CommercialPropertyAreas Areas { get; set; }
    
        /// <summary>
        /// Int&#228;kter
        /// </summary>
        [DataMember(Name="income",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate.Income Income { get; set; }
    
        /// <summary>
        /// Vakanser
        /// </summary>
        [DataMember(Name="vacancies",EmitDefaultValue=false)]
        public string Vacancies { get; set; }
    
        /// <summary>
        /// &#214;vriga kommentarer
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Summa yta
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double? Area { get; set; }
    
        /// <summary>
        /// Summa hyresint&#228;kter
        /// </summary>
        [DataMember(Name="rentIncomes",EmitDefaultValue=false)]
        public double? RentIncomes { get; set; }
    
        /// <summary>
        /// Summa &#246;vriga int&#228;kter
        /// </summary>
        [DataMember(Name="otherIncomes",EmitDefaultValue=false)]
        public double? OtherIncomes { get; set; }
    
        /// <summary>
        /// Summa schabloniserad driftkostnad
        /// </summary>
        [DataMember(Name="operatingCosts",EmitDefaultValue=false)]
        public double? OperatingCosts { get; set; }
    
    }
}