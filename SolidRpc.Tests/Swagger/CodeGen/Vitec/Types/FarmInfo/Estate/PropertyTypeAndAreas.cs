using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PropertyTypeAndAreas {
        /// <summary>
        /// Objekttyp (S�kbegrepp)
        /// </summary>
        [DataMember(Name="propertyType",EmitDefaultValue=false)]
        public string PropertyType { get; set; }
    
        /// <summary>
        /// Inriktning
        /// </summary>
        [DataMember(Name="alignment",EmitDefaultValue=false)]
        public string Alignment { get; set; }
    
        /// <summary>
        /// Antal skiften
        /// </summary>
        [DataMember(Name="numberOfpartition",EmitDefaultValue=false)]
        public int NumberOfpartition { get; set; }
    
        /// <summary>
        /// Areal p� antal skiften
        /// </summary>
        [DataMember(Name="area",EmitDefaultValue=false)]
        public double Area { get; set; }
    
        /// <summary>
        /// Samtaxerade fastigheter
        /// </summary>
        [DataMember(Name="jointlyTaxedProperties",EmitDefaultValue=false)]
        public string JointlyTaxedProperties { get; set; }
    
        /// <summary>
        /// Arealuppgifter
        /// </summary>
        [DataMember(Name="areaData",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.FarmInfo.Estate.AreaData> AreaData { get; set; }
    
        /// <summary>
        /// Vilken arealk�lla som skall visas p� internet.
        /// </summary>
        [DataMember(Name="showAreaDataOnInternet",EmitDefaultValue=false)]
        public string ShowAreaDataOnInternet { get; set; }
    
    }
}