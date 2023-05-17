namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// 
    /// </summary>
    public static class RAMspecsExtensions {
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIContactProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IContact>,RA.Mspecs.Services.IContact {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIContactProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IContact> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="useCache"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Contact.Contact> GetContactAsync(
                System.String id,
                System.Boolean? useCache,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetContactAsync(id, useCache, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contact"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Contact.Contact> UpsertContactAsync(
                RA.Mspecs.Types.Contact.Contact contact,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpsertContactAsync(contact, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIDealProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IDeal>,RA.Mspecs.Services.IDeal {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIDealProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IDeal> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Deal.Deal>> GetDealsAsync(
                System.String subscriberId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetDealsAsync(subscriberId, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="dealId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Deal.Deal> GetDealAsync(
                System.String subscriberId,
                System.String dealId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetDealAsync(subscriberId, dealId, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="dealId"></param>
            /// <param name="bidInput"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Deal.DealBiddingBid> PlaceBidAsync(
                System.String subscriberId,
                System.String dealId,
                RA.Mspecs.Types.Deal.Input.BidInput bidInput,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().PlaceBidAsync(subscriberId, dealId, bidInput, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="dealId"></param>
            /// <param name="bidderId"></param>
            /// <param name="url"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetBidUrlAsync(
                System.String subscriberId,
                System.String dealId,
                System.String bidderId,
                System.Uri url,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().SetBidUrlAsync(subscriberId, dealId, bidderId, url, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="dealId"></param>
            /// <param name="bidderId"></param>
            /// <param name="institute"></param>
            /// <param name="validUntil"></param>
            /// <param name="information"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetLoanPromiseAsync(
                System.String subscriberId,
                System.String dealId,
                System.String bidderId,
                System.String institute,
                System.DateTimeOffset? validUntil,
                System.String information,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().SetLoanPromiseAsync(subscriberId, dealId, bidderId, institute, validUntil, information, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIEstateProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IEstate>,RA.Mspecs.Services.IEstate {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIEstateProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IEstate> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dealId"></param>
            /// <param name="useCache"></param>
            /// <param name="fetchMissingEstate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> GetEstateByDealIdAsync(
                System.String dealId,
                System.Boolean? useCache,
                System.Boolean? fetchMissingEstate,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetEstateByDealIdAsync(dealId, useCache, fetchMissingEstate, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="bidderId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> GetEstateByBidderIdAsync(
                System.String bidderId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetEstateByBidderIdAsync(bidderId, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> UpsertEstateAsync(
                RA.Mspecs.Types.Estate.Estate estate,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpsertEstateAsync(estate, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dealId"></param>
            /// <param name="contactId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> UpdateEstateAsync(
                System.String dealId,
                System.String contactId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpdateEstateAsync(dealId, contactId, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="dealId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> StoreDealByIdAsync(
                System.String subscriberId,
                System.String dealId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().StoreDealByIdAsync(subscriberId, dealId, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="deal"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> StoreDealAsync(
                RA.Mspecs.Types.Deal.Deal deal,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().StoreDealAsync(deal, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dealId"></param>
            /// <param name="contactIds"></param>
            /// <param name="amount"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> PlaceBidAsync(
                System.String dealId,
                System.Collections.Generic.IEnumerable<System.String> contactIds,
                System.Decimal amount,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().PlaceBidAsync(dealId, contactIds, amount, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dealId"></param>
            /// <param name="contactId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetBidderUrlAsync(
                System.String dealId,
                System.String contactId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().SetBidderUrlAsync(dealId, contactId, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dealId"></param>
            /// <param name="contactId"></param>
            /// <param name="institute"></param>
            /// <param name="validUntil"></param>
            /// <param name="information"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetLoanPromiseAsync(
                System.String dealId,
                System.String contactId,
                System.String institute,
                System.DateTimeOffset? validUntil,
                System.String information,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().SetLoanPromiseAsync(dealId, contactId, institute, validUntil, information, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dealId"></param>
            /// <param name="imageId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.FileContent> GetEstateImageAsync(
                System.String dealId,
                System.String imageId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetEstateImageAsync(dealId, imageId, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIEventHandlerProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IEventHandler>,RA.Mspecs.Services.IEventHandler {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIEventHandlerProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IEventHandler> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidderCreatedAsync(
                RA.Mspecs.Types.Event.EventBidderCreated e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleBidderCreatedAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidderUpdatedAsync(
                RA.Mspecs.Types.Event.EventBidderUpdated e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleBidderUpdatedAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleActivateAsync(
                RA.Mspecs.Types.Event.EventDealActivate e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleActivateAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleDeactivateAsync(
                RA.Mspecs.Types.Event.EventDealDeactivate e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleDeactivateAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleUpdateAsync(
                RA.Mspecs.Types.Event.EventDealUpdate e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleUpdateAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidUpdatedAsync(
                RA.Mspecs.Types.Event.EventBidUpdated e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleBidUpdatedAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBiddingUpdatedAsync(
                RA.Mspecs.Types.Event.EventBiddingUpdated e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleBiddingUpdatedAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBiddingRestartAsync(
                RA.Mspecs.Types.Event.EventBiddingRestart e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleBiddingRestartAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidCreatedAsync(
                RA.Mspecs.Types.Event.EventBidCreated e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleBidCreatedAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidWinningAsync(
                RA.Mspecs.Types.Event.EventBidWinning e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleBidWinningAsync(e, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleSubscriberAddedAsync(
                RA.Mspecs.Types.Event.EventSubscriberAdded e,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().HandleSubscriberAddedAsync(e, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIMspecsProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IMspecs>,RA.Mspecs.Services.IMspecs {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIMspecsProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IMspecs> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ProcessWebhookAsync(
                RA.Mspecs.Types.HttpRequest request,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().ProcessWebhookAsync(request, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task InterpretWebhookAsync(
                System.Guid uid,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().InterpretWebhookAsync(uid, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.StoredHttpRequest>> ListWebhookCallsAsync(
                System.DateTimeOffset? fromDate,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().ListWebhookCallsAsync(fromDate, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.FileContent> DownloadWebhookCallsAsync(
                System.DateTimeOffset? fromDate,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().DownloadWebhookCallsAsync(fromDate, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Service.Service>> GetServicesAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetServicesAsync(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="service"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateServiceAsync(
                RA.Mspecs.Types.Service.Service service,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpdateServiceAsync(service, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetWebhooksOnServicesAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().SetWebhooksOnServicesAsync(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Subscriber.Subscriber>> GetSubscribersAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetSubscribersAsync(cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIOfficeProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IOffice>,RA.Mspecs.Services.IOffice {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIOfficeProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IOffice> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="useCache"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Office.Office> GetOfficeByIdAsync(
                System.String id,
                System.Boolean? useCache,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetOfficeByIdAsync(id, useCache, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="office"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Office.Office> UpsertOfficeAsync(
                RA.Mspecs.Types.Office.Office office,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpsertOfficeAsync(office, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIRedirectProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IRedirect>,RA.Mspecs.Services.IRedirect {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIRedirectProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IRedirect> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="bidderId"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.FileContent> RedirectBidderAsync(
                System.String bidderId,
                System.Threading.CancellationToken cancellation) {
                return GetImplementation().RedirectBidderAsync(bidderId, cancellation);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIUpdateHandlerProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IUpdateHandler>,RA.Mspecs.Services.IUpdateHandler {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIUpdateHandlerProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IUpdateHandler> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Customer.CustomerEndpoint>> GetCustomerEndpointsAsync(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetCustomerEndpointsAsync(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="useCache"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Customer.CustomerEndpoint> GetCustomerEndpointAsync(
                System.String subscriberId,
                System.Boolean? useCache,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetCustomerEndpointAsync(subscriberId, useCache, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="customerEndpoint"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertCustomerEndpointAsync(
                RA.Mspecs.Types.Customer.CustomerEndpoint customerEndpoint,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpsertCustomerEndpointAsync(customerEndpoint, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dataType"></param>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task StructureUpdatedAsync(
                System.String dataType,
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().StructureUpdatedAsync(dataType, id, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RAMspecsServicesIUserProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Mspecs.Services.IUser>,RA.Mspecs.Services.IUser {
            /// <summary>
            /// 
            /// </summary>
            public RAMspecsServicesIUserProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Mspecs.Services.IUser> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="useCache"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.User.User> GetUserByIdAsync(
                System.String id,
                System.Boolean? useCache,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetUserByIdAsync(id, useCache, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="user"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.User.User> UpsertUserAsync(
                RA.Mspecs.Types.User.User user,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpsertUserAsync(user, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.FileContent> GetUserImageAsync(
                System.String userId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetUserImageAsync(userId, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddRAMspecs(
            this Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<RA.Mspecs.Services.IContact,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIContactProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IDeal,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIDealProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IEstate,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIEstateProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IEventHandler,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIEventHandlerProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IMspecs,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIMspecsProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IOffice,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIOfficeProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IRedirect,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIRedirectProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IUpdateHandler,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIUpdateHandlerProxy>(configure);
            sc.SetupProxy<RA.Mspecs.Services.IUser,Microsoft.Extensions.DependencyInjection.RAMspecsExtensions.RAMspecsServicesIUserProxy>(configure);
            return sc;
        }
    
    }
}