using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models;
using System.Threading;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Update.Contact;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IContacts {
        /// <summary>
        /// H�mtar lista �ver kontakter. H�mta kontaktlista.
        /// F�r att kunna h�mta en kontaktlista s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteria">Urval</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<ContactCollection>> ContactsGetContacts(
            ContactCriteria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar lista �ver kontakters id och �ndringsdatum. H�mta kontaktlista.
        /// F�r att kunna h�mta en kontaktlista s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteria">Urval</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<ContactCollectionForSyncList>> ContactsGetContactsSyncList(
            ContactCriteria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar lista av kontaktid med telfonnummer
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
        /// Dubblettkontroll sker p� &lt;br /&gt;
        /// 1) kontaktid, 2) personnummer, 3) f�rnamn och efternamn, och n�got av telefonnummer, mobilnummer eller epostadress.&lt;br /&gt;
        /// Matar man in information i n�got av ovanst�ende f�lt, s� fylls �vriga f�lt i automatiskt.&lt;br /&gt;
        /// Minst ett f�lt beh�ver vara ifyllt f�r att dubblettkontroll ska kunna g�ras. Om dubblettkontrollen inte hittar en redan befintlig kontakt skapas en ny. Skicka in ny eller uppdatera befintlig kontaktperson. &lt;br /&gt;&lt;br /&gt;
        /// F�r att kunna uppdatera eller skapa en kontakt s� kr�vs det en giltig API nyckel och ett kundid (Customerld).&lt;br /&gt; Finns Contactld (kontaktid) sker kontrollen p� detta och alla inskickade f�lt uppdateras p� kontakten.&lt;br /&gt; Saknas Contactld (kontaktid) sker kontrollen p� SocialSecurityNumber (personummer) och alla �vriga f�lt p� konten uppdateras.&lt;br /&gt; Finns ingen av ovanst�nde f�lt s� sker kontrollen p� FirstName (f�rnamn) och LastName (efternamn) och dessutom p� n�got av  telephone (telefonnummer), cellphone (mobilnummer) eller EmailAddress (Epostadress1). L�mnas dessa f�lt tomma sker ingen dubblett kontroll men minst ett av dessa m�ste skickas in f�r att identifiera en dubblett. Hittas ingen dubblett skapas en ny person.
        /// </summary>
        /// <param name="contact">Kontakt</param>
        /// <param name="cancellationToken"></param>
        Task<string> ContactsUpdatePerson(
            UpdatePerson contact,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapar en ny eller uppdaterar en befintlig f�retagskontakt.&lt;br /&gt;Dubblettkontroll sker p�&lt;br /&gt; 1) kontaktid, 2) organisationsnummer, 3) f�retagsnamn, telefonnummer eller epostadress. Matar man in information i n�got av ovanst�ende f�lt, s� fylls �vriga f�lt i automatiskt. Minst ett f�lt beh�ver vara ifyllt f�r att dubblettkontroll ska kunna g�ras. Om dubblettkontrollen inte hittar en redan befintlig kontakt skapas en ny. Skicka in ny eller uppdatera befintlig f�retagskontakt.&lt;br /&gt;&lt;br /&gt;
        /// F�r att kunna uppdatera eller skapa en kontakt s� kr�vs det en giltig API nyckel och ett kundid (Customerld).&lt;br /&gt;
        /// Finns Contactld (kontaktid) sker kontrollen p� detta och alla inskickade f�lt uppdateras p� f�retaget. &lt;br /&gt;
        /// Saknas Contactld (kontaktid) sker kontrollen p� CorporateHumber (organisationsnummer) och alla �vriga f�lt p� f�retaget uppdateras. &lt;br /&gt;
        /// Finns ingen av ovanst�ende f�lt sker kontrollen p� CompanyHame (f�retagsnamn) och dessutom p� SwitchPhone (telefonnummer) eller EmailAddress (Epostadressl). L�mnas dessa f�lt tomma sker ingen dubblett kontroll men minst en m�ste skickas in f�r att identifiera en dubblett. Hittas ingen dubblett skapas ett nytt f�retag.
        /// </summary>
        /// <param name="contact">Kontakt</param>
        /// <param name="cancellationToken"></param>
        Task<string> ContactsUpdateCompany(
            UpdateCompany contact,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}