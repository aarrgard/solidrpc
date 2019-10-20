using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.Meeting.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Meeting {
        /// <summary>
        /// Kontakt id
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Kontaktnamn
        /// </summary>
        [DataMember(Name="contactName",EmitDefaultValue=false)]
        public string ContactName { get; set; }
    
        /// <summary>
        /// Adress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public Address Address { get; set; }
    
        /// <summary>
        /// Telefonnummer bostad
        /// </summary>
        [DataMember(Name="telePhone",EmitDefaultValue=false)]
        public string TelePhone { get; set; }
    
        /// <summary>
        /// Telefonnummer
        /// </summary>
        [DataMember(Name="cellPhone",EmitDefaultValue=false)]
        public string CellPhone { get; set; }
    
        /// <summary>
        /// M�tesdatum
        /// </summary>
        [DataMember(Name="meetingDate",EmitDefaultValue=false)]
        public DateTimeOffset MeetingDate { get; set; }
    
        /// <summary>
        /// M�klarens id
        /// </summary>
        [DataMember(Name="brokerId",EmitDefaultValue=false)]
        public string BrokerId { get; set; }
    
        /// <summary>
        /// M�klarens namn
        /// </summary>
        [DataMember(Name="brokerName",EmitDefaultValue=false)]
        public string BrokerName { get; set; }
    
        /// <summary>
        /// Bokarens id
        /// </summary>
        [DataMember(Name="bookedById",EmitDefaultValue=false)]
        public string BookedById { get; set; }
    
        /// <summary>
        /// Bokarens name
        /// </summary>
        [DataMember(Name="bookedByName",EmitDefaultValue=false)]
        public string BookedByName { get; set; }
    
        /// <summary>
        /// Bokningsdatum
        /// </summary>
        [DataMember(Name="bookedDate",EmitDefaultValue=false)]
        public DateTimeOffset BookedDate { get; set; }
    
        /// <summary>
        /// P�skriftsdatum
        /// </summary>
        [DataMember(Name="signatureDate",EmitDefaultValue=false)]
        public DateTimeOffset SignatureDate { get; set; }
    
        /// <summary>
        /// Objektets status
        /// </summary>
        [DataMember(Name="estateStatus",EmitDefaultValue=false)]
        public Status EstateStatus { get; set; }
    
        /// <summary>
        /// Provision ex.moms
        /// </summary>
        [DataMember(Name="commissionWithoutTaxes",EmitDefaultValue=false)]
        public double CommissionWithoutTaxes { get; set; }
    
        /// <summary>
        /// Intagsk�lla (Kontakttyp)
        /// </summary>
        [DataMember(Name="assignmentSource",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Meeting.Models.AssignmentSource AssignmentSource { get; set; }
    
        /// <summary>
        /// Avbokad av (M�klare, Kund)
        /// </summary>
        [DataMember(Name="cancelledBy",EmitDefaultValue=false)]
        public string CancelledBy { get; set; }
    
        /// <summary>
        /// Kontraktsdatum
        /// </summary>
        [DataMember(Name="agreementDate",EmitDefaultValue=false)]
        public DateTimeOffset AgreementDate { get; set; }
    
        /// <summary>
        /// Tilltr�desdatum
        /// </summary>
        [DataMember(Name="accessDate",EmitDefaultValue=false)]
        public DateTimeOffset AccessDate { get; set; }
    
        /// <summary>
        /// Objektets id
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
    }
}