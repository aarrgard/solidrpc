using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CompilationAreas {
        /// <summary>
        /// Areak�lla
        /// </summary>
        [DataMember(Name="areaSource",EmitDefaultValue=false)]
        public string AreaSource { get; set; }
    
        /// <summary>
        /// Bost�der
        /// </summary>
        [DataMember(Name="residential",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Residential { get; set; }
    
        /// <summary>
        /// Garage
        /// </summary>
        [DataMember(Name="garage",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Garage { get; set; }
    
        /// <summary>
        /// P-plats
        /// </summary>
        [DataMember(Name="parkingLot",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea ParkingLot { get; set; }
    
        /// <summary>
        /// Kontor
        /// </summary>
        [DataMember(Name="office",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Office { get; set; }
    
        /// <summary>
        /// Butiker
        /// </summary>
        [DataMember(Name="stores",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Stores { get; set; }
    
        /// <summary>
        /// Industri/verkstad
        /// </summary>
        [DataMember(Name="industry",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea Industry { get; set; }
    
        /// <summary>
        /// Varmlager
        /// </summary>
        [DataMember(Name="warmStockroom",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea WarmStockroom { get; set; }
    
        /// <summary>
        /// Kallager
        /// </summary>
        [DataMember(Name="coldStockroom",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationArea ColdStockroom { get; set; }
    
        /// <summary>
        /// �vrigt 1
        /// </summary>
        [DataMember(Name="other1",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationAreaOther Other1 { get; set; }
    
        /// <summary>
        /// �vrigt 2
        /// </summary>
        [DataMember(Name="other2",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationAreaOther Other2 { get; set; }
    
        /// <summary>
        /// �vriga areor
        /// </summary>
        [DataMember(Name="others",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CompilationAreaOther> Others { get; set; }
    
        /// <summary>
        /// Areor
        /// </summary>
        [DataMember(Name="areas",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.CommercialPropertyAreas Areas { get; set; }
    
        /// <summary>
        /// Int�kter
        /// </summary>
        [DataMember(Name="income",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate.Income Income { get; set; }
    
        /// <summary>
        /// Vakanser
        /// </summary>
        [DataMember(Name="vacancies",EmitDefaultValue=false)]
        public string Vacancies { get; set; }
    
        /// <summary>
        /// �vriga kommentarer
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// Summa yta
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double Area { get; set; }
    
        /// <summary>
        /// Summa hyresint�kter
        /// </summary>
        [DataMember(Name="rentIncomes",EmitDefaultValue=false)]
        public double RentIncomes { get; set; }
    
        /// <summary>
        /// Summa �vriga int�kter
        /// </summary>
        [DataMember(Name="otherIncomes",EmitDefaultValue=false)]
        public double OtherIncomes { get; set; }
    
        /// <summary>
        /// Summa schabloniserad driftkostnad
        /// </summary>
        [DataMember(Name="operatingCosts",EmitDefaultValue=false)]
        public double OperatingCosts { get; set; }
    
    }
}