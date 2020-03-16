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
        /// H&#228;mta lista av kontakter som matchar ett kriterie.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="criteriaAgentId">Urval på användareid</param>
        /// <param name="criteriaCreatedAtFrom">Skapad från och med</param>
        /// <param name="criteriaCreatedAtTo">Skapad till och med</param>
        /// <param name="criteriaChangedAtFrom">Ändrad från och med</param>
        /// <param name="criteriaChangedAtTo">Ändrad till och med</param>
        /// <param name="criteriaCustomFieldName">Egendefinerat fältnamn</param>
        /// <param name="criteriaCustomFieldValue">Egendefinerat fältvärde</param>
        /// <param name="cancellationToken"></param>
        Task<CrmContactList> CrmContactSelect(
            string customerId,
            string criteriaAgentId = null,
            DateTimeOffset? criteriaCreatedAtFrom = null,
            DateTimeOffset? criteriaCreatedAtTo = null,
            DateTimeOffset? criteriaChangedAtFrom = null,
            DateTimeOffset? criteriaChangedAtTo = null,
            string criteriaCustomFieldName = null,
            string criteriaCustomFieldValue = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta kontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmContact> CrmContactGet(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista utav kontakter, max 20 stycken &#229;t g&#229;ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmContact>> CrmContactGetList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta f&#246;retagskontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmCompanyContact> CrmContactGetCompany(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista utav f&#246;retagskontakter, max 20 stycken &#229;t g&#229;ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmCompanyContact>> CrmContactGetCompanyList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta personkontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmPersonContact> CrmContactGetPerson(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista utav personkontakter, max 20 stycken &#229;t g&#229;ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmPersonContact>> CrmContactGetPersonList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta d&#246;dsbokontakt
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmDeceasedEstateContact> CrmContactGetDeceasedEstate(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista utav d&#246;dsbokontakter, max 20 stycken &#229;t g&#229;ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmDeceasedEstateContact>> CrmContactGetDeceasedEstateList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta nuvarande boende f&#246;r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmPresentAccomodation> CrmContactGetPresentAccomodation(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista av nuvarande boenden f&#246;r kontakter, max 20 stycken kontakter &#229;t g&#229;ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmPresentAccomodation>> CrmContactGetPresentAccomodationList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta spekulantrelationerna f&#246;r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmSpeculatorContact> CrmContactGetSpeculatorRelations(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista av spekulantrelationer f&#246;r kontakter, max 20 stycken kontakter &#229;t g&#229;ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmSpeculatorContact>> CrmContactGetSpeculatorRelationsList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta k&#246;parrelationerna f&#246;r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmBuyerContact> CrmContactGetBuyerRelations(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista av k&#246;parrelationer f&#246;r kontakter, max 20 stycken kontakter &#229;t g&#229;ngen.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactIds">Kontaktidn (kommaseparerade, max 20 stycken).</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmBuyerContact>> CrmContactGetBuyerRelationsList(
            string customerId,
            string contactIds,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta s&#228;ljarrelationerna f&#246;r en kontakt.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="contactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<CrmSellerContact> CrmContactGetSellerRelations(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta lista av s&#228;ljarrelationer f&#246;r kontakter, max 20 stycken kontakter &#229;t g&#229;ngen.
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
        /// Ta bort en kategori fr&#229;n en kontakt.
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