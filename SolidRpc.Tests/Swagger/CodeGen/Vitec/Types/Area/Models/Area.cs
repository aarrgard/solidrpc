using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Area.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Area {
        /// <summary>
        /// Omr�dets id
        /// </summary>
        [DataMember(Name="areaId",EmitDefaultValue=false)]
        public string AreaId { get; set; }
    
        /// <summary>
        /// Arkiverad
        /// </summary>
        [DataMember(Name="archived",EmitDefaultValue=false)]
        public bool Archived { get; set; }
    
        /// <summary>
        /// Externt id
        /// </summary>
        [DataMember(Name="externalId",EmitDefaultValue=false)]
        public string ExternalId { get; set; }
    
        /// <summary>
        /// Omr�desnamn
        /// </summary>
        [DataMember(Name="areaName",EmitDefaultValue=false)]
        public string AreaName { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Visa p� internet
        /// </summary>
        [DataMember(Name="showOnInternet",EmitDefaultValue=false)]
        public bool ShowOnInternet { get; set; }
    
        /// <summary>
        /// Omgivning
        /// </summary>
        [DataMember(Name="surrounding",EmitDefaultValue=false)]
        public Surrounding Surrounding { get; set; }
    
        /// <summary>
        /// LK-kod
        /// </summary>
        [DataMember(Name="county",EmitDefaultValue=false)]
        public string County { get; set; }
    
        /// <summary>
        /// Omr�det som 2d-polygon
        /// </summary>
        [DataMember(Name="polygon",EmitDefaultValue=false)]
        public IEnumerable<Coordinate> Polygon { get; set; }
    
        /// <summary>
        /// Omr�det som 2d-polygon i WGS84
        /// </summary>
        [DataMember(Name="polygonWGS84",EmitDefaultValue=false)]
        public IEnumerable<Coordinate> PolygonWGS84 { get; set; }
    
        /// <summary>
        /// Bilder p� omr�det
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Area.Models.AreaImage> Images { get; set; }
    
        /// <summary>
        /// �ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset DateChanged { get; set; }
    
    }
}