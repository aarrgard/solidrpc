using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using System;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Viewing.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class UpdatePerson {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="categoryIds",EmitDefaultValue=false)]
        public IEnumerable<string> CategoryIds { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public Address Address { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public Email Email { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="otherPhone",EmitDefaultValue=false)]
        public string OtherPhone { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="wishAdvertising",EmitDefaultValue=false)]
        public bool? WishAdvertising { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="coordinate",EmitDefaultValue=false)]
        public Coordinate Coordinate { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="task",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Task.Models.Task Task { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="customField",EmitDefaultValue=false)]
        public FieldValueCriteria CustomField { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="firstName",EmitDefaultValue=false)]
        public string FirstName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="lastName",EmitDefaultValue=false)]
        public string LastName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="socialSecurityNumber",EmitDefaultValue=false)]
        public string SocialSecurityNumber { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="telePhone",EmitDefaultValue=false)]
        public string TelePhone { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="workPhone",EmitDefaultValue=false)]
        public string WorkPhone { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="cellPhone",EmitDefaultValue=false)]
        public string CellPhone { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="approval",EmitDefaultValue=false)]
        public bool? Approval { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="approvalDate",EmitDefaultValue=false)]
        public DateTimeOffset? ApprovalDate { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="gdprApprovalDate",EmitDefaultValue=false)]
        public DateTimeOffset? GdprApprovalDate { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="obtainThrough",EmitDefaultValue=false)]
        public string ObtainThrough { get; set; }
    
    }
}