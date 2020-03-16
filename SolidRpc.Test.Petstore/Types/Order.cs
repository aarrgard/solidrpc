using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Petstore.Types {
    /// <summary>
    /// successful operation
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Order {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="petId",EmitDefaultValue=false)]
        public long PetId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="quantity",EmitDefaultValue=false)]
        public int Quantity { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="shipDate",EmitDefaultValue=false)]
        public DateTimeOffset ShipDate { get; set; }
    
        /// <summary>
        /// Order Status
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="complete",EmitDefaultValue=false)]
        public bool Complete { get; set; }
    
    }
}