using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Estate;
using System;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.List.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FarmBasic {
        /// <summary>
        /// Boarea
        /// </summary>
        [DataMember(Name="livingSpace",EmitDefaultValue=false)]
        public double LivingSpace { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="rooms",EmitDefaultValue=false)]
        public double Rooms { get; set; }
    
        /// <summary>
        /// Pris
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public double Price { get; set; }
    
        /// <summary>
        /// Bostadsid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public Status Status { get; set; }
    
        /// <summary>
        /// Huvudtillh�righet
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Omr�des namn
        /// </summary>
        [DataMember(Name="areaName",EmitDefaultValue=false)]
        public string AreaName { get; set; }
    
        /// <summary>
        /// �ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset DateChanged { get; set; }
    
        /// <summary>
        /// Gatuadress
        /// </summary>
        [DataMember(Name="streetAddress",EmitDefaultValue=false)]
        public string StreetAddress { get; set; }
    
        /// <summary>
        /// Kordinater
        /// </summary>
        [DataMember(Name="coordinate",EmitDefaultValue=false)]
        public Coordinate Coordinate { get; set; }
    
        /// <summary>
        /// Kort beskrivning
        /// </summary>
        [DataMember(Name="shortSaleDescription",EmitDefaultValue=false)]
        public string ShortSaleDescription { get; set; }
    
        /// <summary>
        /// Visningars datum
        /// </summary>
        [DataMember(Name="viewingsList",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.List.Estate.ViewingBasic> ViewingsList { get; set; }
    
        /// <summary>
        /// P�g�r bud Ja/Nej
        /// </summary>
        [DataMember(Name="bidding",EmitDefaultValue=false)]
        public bool Bidding { get; set; }
    
        /// <summary>
        /// Objektnummer
        /// </summary>
        [DataMember(Name="objectNumber",EmitDefaultValue=false)]
        public string ObjectNumber { get; set; }
    
        /// <summary>
        /// Huvudbild
        /// </summary>
        [DataMember(Name="mainImage",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Image.Models.Image MainImage { get; set; }
    
    }
}