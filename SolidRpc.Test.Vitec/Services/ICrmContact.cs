using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Crm.Contact;
using System;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmContact {
        /// <summary>
        /// H�mta lista av kontakter som matchar ett kriterie.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="criteriaAgentId">Urval p� anv�ndareid</param>
        /// <param name="criteriaCreatedAtFrom">Skapad fr�n och med</param>
        /// <param name="criteriaCreatedAtTo">Skapad till och med</param>
        /// <param name="criteriaChangedAtFrom">�ndrad fr�n och med</param>
        /// <param name="criteriaChangedAtTo">�ndrad till och med</param>
        /// <param name="criteriaCustomFieldName">Egendefinerat f�ltnamn</param>
        /// <param name="criteriaCustomFieldValue">Egendefinerat f�ltv�rde</param>
        /// <param name="cancellationToken"></param>
        Task<CrmContactList> CrmContactSelect(
            string customerId,
            string criteriaAgentId = default(string),
            DateTimeOffset criteriaCreatedAtFrom = default(DateTimeOffset),
            DateTimeOffset criteriaCreatedAtTo = default(DateTimeOffset),
            DateTimeOffset criteriaChangedAtFrom = default(DateTimeOffset),
            DateTimeOffset criteriaChangedAtTo = default(DateTimeOffset),
            string criteriaCustomFieldName = default(string),
            string criteriaCustomFieldValue = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta kontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmContact> CrmContactGet(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista utav kontakter, max 20 stycken �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmContact>> CrmContactGetList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta f�retagskontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmCompanyContact> CrmContactGetCompany(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista utav f�retagskontakter, max 20 stycken �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmCompanyContact>> CrmContactGetCompanyList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta personkontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmPersonContact> CrmContactGetPerson(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista utav personkontakter, max 20 stycken �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmPersonContact>> CrmContactGetPersonList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta d�dsbokontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmDeceasedEstateContact> CrmContactGetDeceasedEstate(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista utav d�dsbokontakter, max 20 stycken �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmDeceasedEstateContact>> CrmContactGetDeceasedEstateList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta nuvarande boende f�r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmPresentAccomodation> CrmContactGetPresentAccomodation(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista av nuvarande boenden f�r kontakter, max 20 stycken kontakter �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmPresentAccomodation>> CrmContactGetPresentAccomodationList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta spekulantrelationerna f�r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmSpeculatorContact> CrmContactGetSpeculatorRelations(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista av spekulantrelationer f�r kontakter, max 20 stycken kontakter �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmSpeculatorContact>> CrmContactGetSpeculatorRelationsList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta k�parrelationerna f�r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmBuyerContact> CrmContactGetBuyerRelations(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista av k�parrelationer f�r kontakter, max 20 stycken kontakter �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmBuyerContact>> CrmContactGetBuyerRelationsList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta s�ljarrelationerna f�r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmSellerContact> CrmContactGetSellerRelations(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta lista av s�ljarrelationer f�r kontakter, max 20 stycken kontakter �t g�ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmSellerContact>> CrmContactGetSellerRelationsList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Avregistrera utskick
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmContact.CrmContactOptOut.NoContentException">No Content</exception>
        Task CrmContactOptOut(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Ta bort en kategori fr�n en kontakt.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="id">Kategori id</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmContact.CrmContactDeleteCategory.NoContentException">No Content</exception>
        Task CrmContactDeleteCategory(
            string customerId,
            string contactId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}