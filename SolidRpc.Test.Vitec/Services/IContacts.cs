using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Contact.Models;
using System.Threading;
using SolidRpc.Test.Vitec.Types.Update.Contact;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IContacts {
        /// <summary>
        /// H&#228;mtar lista &#246;ver kontakter. H&#228;mta kontaktlista.
        /// F&#246;r att kunna h&#228;mta en kontaktlista s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteria">Urval</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<ContactCollection>> ContactsGetContacts(
            ContactCriteria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar lista &#246;ver kontakters id och &#228;ndringsdatum. H&#228;mta kontaktlista.
        /// F&#246;r att kunna h&#228;mta en kontaktlista s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteria">Urval</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<ContactCollectionForSyncList>> ContactsGetContactsSyncList(
            ContactCriteria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar lista av kontaktid med telfonnummer
        /// </summary>
        /// <param name="phoneNumber">Telefonnummer.</param>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CustomerContactIds>> ContactsGetContactsWithPhonenumber(
            string phoneNumber,
            string customerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapar en ny eller uppdaterar en befintlig kontaktperson.&lt;br /&gt;
        /// Dubblettkontroll sker p&#229; &lt;br /&gt;
        /// 1) kontaktid, 2) personnummer, 3) f&#246;rnamn och efternamn, och n&#229;got av telefonnummer, mobilnummer eller epostadress.&lt;br /&gt;
        /// Matar man in information i n&#229;got av ovanst&#229;ende f&#228;lt, s&#229; fylls &#246;vriga f&#228;lt i automatiskt.&lt;br /&gt;
        /// Minst ett f&#228;lt beh&#246;ver vara ifyllt f&#246;r att dubblettkontroll ska kunna g&#246;ras. Om dubblettkontrollen inte hittar en redan befintlig kontakt skapas en ny. Skicka in ny eller uppdatera befintlig kontaktperson. &lt;br /&gt;&lt;br /&gt;
        /// F&#246;r att kunna uppdatera eller skapa en kontakt s&#229; kr&#228;vs det en giltig API nyckel och ett kundid (Customerld).&lt;br /&gt; Finns Contactld (kontaktid) sker kontrollen p&#229; detta och alla inskickade f&#228;lt uppdateras p&#229; kontakten.&lt;br /&gt; Saknas Contactld (kontaktid) sker kontrollen p&#229; SocialSecurityNumber (personummer) och alla &#246;vriga f&#228;lt p&#229; konten uppdateras.&lt;br /&gt; Finns ingen av ovanst&#229;nde f&#228;lt s&#229; sker kontrollen p&#229; FirstName (f&#246;rnamn) och LastName (efternamn) och dessutom p&#229; n&#229;got av  telephone (telefonnummer), cellphone (mobilnummer) eller EmailAddress (Epostadress1). L&#228;mnas dessa f&#228;lt tomma sker ingen dubblett kontroll men minst ett av dessa m&#229;ste skickas in f&#246;r att identifiera en dubblett. Hittas ingen dubblett skapas en ny person.
        /// </summary>
        /// <param name="contact">Kontakt</param>
        /// <param name="cancellationToken"></param>
        Task<string> ContactsUpdatePerson(
            UpdatePerson contact,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapar en ny eller uppdaterar en befintlig f&#246;retagskontakt.&lt;br /&gt;Dubblettkontroll sker p&#229;&lt;br /&gt; 1) kontaktid, 2) organisationsnummer, 3) f&#246;retagsnamn, telefonnummer eller epostadress. Matar man in information i n&#229;got av ovanst&#229;ende f&#228;lt, s&#229; fylls &#246;vriga f&#228;lt i automatiskt. Minst ett f&#228;lt beh&#246;ver vara ifyllt f&#246;r att dubblettkontroll ska kunna g&#246;ras. Om dubblettkontrollen inte hittar en redan befintlig kontakt skapas en ny. Skicka in ny eller uppdatera befintlig f&#246;retagskontakt.&lt;br /&gt;&lt;br /&gt;
        /// F&#246;r att kunna uppdatera eller skapa en kontakt s&#229; kr&#228;vs det en giltig API nyckel och ett kundid (Customerld).&lt;br /&gt;
        /// Finns Contactld (kontaktid) sker kontrollen p&#229; detta och alla inskickade f&#228;lt uppdateras p&#229; f&#246;retaget. &lt;br /&gt;
        /// Saknas Contactld (kontaktid) sker kontrollen p&#229; CorporateHumber (organisationsnummer) och alla &#246;vriga f&#228;lt p&#229; f&#246;retaget uppdateras. &lt;br /&gt;
        /// Finns ingen av ovanst&#229;ende f&#228;lt sker kontrollen p&#229; CompanyHame (f&#246;retagsnamn) och dessutom p&#229; SwitchPhone (telefonnummer) eller EmailAddress (Epostadressl). L&#228;mnas dessa f&#228;lt tomma sker ingen dubblett kontroll men minst en m&#229;ste skickas in f&#246;r att identifiera en dubblett. Hittas ingen dubblett skapas ett nytt f&#246;retag.
        /// </summary>
        /// <param name="contact">Kontakt</param>
        /// <param name="cancellationToken"></param>
        Task<string> ContactsUpdateCompany(
            UpdateCompany contact,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}