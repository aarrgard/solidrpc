using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateList {
        /// <summary>
        /// Kund id
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Lista p&#229; villor
        /// </summary>
        [DataMember(Name="houses",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.HouseBasic> Houses { get; set; }
    
        /// <summary>
        /// Lista p&#229; fritidsvillor
        /// </summary>
        [DataMember(Name="cottages",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.HouseBasic> Cottages { get; set; }
    
        /// <summary>
        /// Lista p&#229; bostadsr&#228;tter
        /// </summary>
        [DataMember(Name="housingCooperativeses",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.HousingCooperativesBasic> HousingCooperativeses { get; set; }
    
        /// <summary>
        /// Lista p&#229; tomter
        /// </summary>
        [DataMember(Name="plots",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.PlotBasic> Plots { get; set; }
    
        /// <summary>
        /// Lista p&#229; projekt
        /// </summary>
        [DataMember(Name="projects",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.ProjectBasic> Projects { get; set; }
    
        /// <summary>
        /// Lista p&#229; g&#229;rdar
        /// </summary>
        [DataMember(Name="farms",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.FarmBasic> Farms { get; set; }
    
        /// <summary>
        /// Lista p&#229; kommersiella objekt
        /// </summary>
        [DataMember(Name="commercialPropertys",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.CommercialPropertyBasic> CommercialPropertys { get; set; }
    
        /// <summary>
        /// Lista p&#229; &#228;garl&#228;genheter
        /// </summary>
        [DataMember(Name="condominiums",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.CondominiumBasic> Condominiums { get; set; }
    
        /// <summary>
        /// Lista p&#229; utlandsbost&#228;der
        /// </summary>
        [DataMember(Name="foreignProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.ForeignPropertyBasic> ForeignProperties { get; set; }
    
        /// <summary>
        /// Lista p&#229; lokaler
        /// </summary>
        [DataMember(Name="premises",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate.PremisesBasic> Premises { get; set; }
    
    }
}