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
            System.Reflection.MethodInfo mi_GetContactAsync_id_useCache_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Contact.Contact>(_serviceProvider, impl, mi_GetContactAsync_id_useCache_cancellationToken, new object[] {id, useCache, cancellationToken}, () => impl.GetContactAsync(id, useCache, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpsertContactAsync_contact_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contact"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Contact.Contact> UpsertContactAsync(
                RA.Mspecs.Types.Contact.Contact contact,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Contact.Contact>(_serviceProvider, impl, mi_UpsertContactAsync_contact_cancellationToken, new object[] {contact, cancellationToken}, () => impl.UpsertContactAsync(contact, cancellationToken));
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
            System.Reflection.MethodInfo mi_GetDealsAsync_subscriberId_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="subscriberId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Deal.Deal>> GetDealsAsync(
                System.String subscriberId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IDeal)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Deal.Deal>>(_serviceProvider, impl, mi_GetDealsAsync_subscriberId_cancellationToken, new object[] {subscriberId, cancellationToken}, () => impl.GetDealsAsync(subscriberId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetDealAsync_subscriberId_dealId_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IDeal)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Deal.Deal>(_serviceProvider, impl, mi_GetDealAsync_subscriberId_dealId_cancellationToken, new object[] {subscriberId, dealId, cancellationToken}, () => impl.GetDealAsync(subscriberId, dealId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_PlaceBidAsync_subscriberId_dealId_bidInput_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IDeal)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Deal.DealBiddingBid>(_serviceProvider, impl, mi_PlaceBidAsync_subscriberId_dealId_bidInput_cancellationToken, new object[] {subscriberId, dealId, bidInput, cancellationToken}, () => impl.PlaceBidAsync(subscriberId, dealId, bidInput, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_SetBidUrlAsync_subscriberId_dealId_bidderId_url_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IDeal)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetBidUrlAsync_subscriberId_dealId_bidderId_url_cancellationToken, new object[] {subscriberId, dealId, bidderId, url, cancellationToken}, () => impl.SetBidUrlAsync(subscriberId, dealId, bidderId, url, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_SetLoanPromiseAsync_subscriberId_dealId_bidderId_institute_validUntil_information_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IDeal)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetLoanPromiseAsync_subscriberId_dealId_bidderId_institute_validUntil_information_cancellationToken, new object[] {subscriberId, dealId, bidderId, institute, validUntil, information, cancellationToken}, () => impl.SetLoanPromiseAsync(subscriberId, dealId, bidderId, institute, validUntil, information, cancellationToken));
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
            System.Reflection.MethodInfo mi_GetEstateByDealIdAsync_dealId_useCache_fetchMissingEstate_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Estate.Estate>(_serviceProvider, impl, mi_GetEstateByDealIdAsync_dealId_useCache_fetchMissingEstate_cancellationToken, new object[] {dealId, useCache, fetchMissingEstate, cancellationToken}, () => impl.GetEstateByDealIdAsync(dealId, useCache, fetchMissingEstate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetEstateByBidderIdAsync_bidderId_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="bidderId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> GetEstateByBidderIdAsync(
                System.String bidderId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Estate.Estate>(_serviceProvider, impl, mi_GetEstateByBidderIdAsync_bidderId_cancellationToken, new object[] {bidderId, cancellationToken}, () => impl.GetEstateByBidderIdAsync(bidderId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpsertEstateAsync_estate_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> UpsertEstateAsync(
                RA.Mspecs.Types.Estate.Estate estate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Estate.Estate>(_serviceProvider, impl, mi_UpsertEstateAsync_estate_cancellationToken, new object[] {estate, cancellationToken}, () => impl.UpsertEstateAsync(estate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpdateEstateAsync_dealId_contactId_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Estate.Estate>(_serviceProvider, impl, mi_UpdateEstateAsync_dealId_contactId_cancellationToken, new object[] {dealId, contactId, cancellationToken}, () => impl.UpdateEstateAsync(dealId, contactId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_StoreDealByIdAsync_subscriberId_dealId_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Estate.Estate>(_serviceProvider, impl, mi_StoreDealByIdAsync_subscriberId_dealId_cancellationToken, new object[] {subscriberId, dealId, cancellationToken}, () => impl.StoreDealByIdAsync(subscriberId, dealId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_StoreDealAsync_deal_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="deal"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Estate.Estate> StoreDealAsync(
                RA.Mspecs.Types.Deal.Deal deal,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Estate.Estate>(_serviceProvider, impl, mi_StoreDealAsync_deal_cancellationToken, new object[] {deal, cancellationToken}, () => impl.StoreDealAsync(deal, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_PlaceBidAsync_dealId_contactIds_amount_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Estate.Estate>(_serviceProvider, impl, mi_PlaceBidAsync_dealId_contactIds_amount_cancellationToken, new object[] {dealId, contactIds, amount, cancellationToken}, () => impl.PlaceBidAsync(dealId, contactIds, amount, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_SetBidderUrlAsync_dealId_contactId_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetBidderUrlAsync_dealId_contactId_cancellationToken, new object[] {dealId, contactId, cancellationToken}, () => impl.SetBidderUrlAsync(dealId, contactId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_SetLoanPromiseAsync_dealId_contactId_institute_validUntil_information_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetLoanPromiseAsync_dealId_contactId_institute_validUntil_information_cancellationToken, new object[] {dealId, contactId, institute, validUntil, information, cancellationToken}, () => impl.SetLoanPromiseAsync(dealId, contactId, institute, validUntil, information, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetEstateImageAsync_dealId_imageId_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.FileContent>(_serviceProvider, impl, mi_GetEstateImageAsync_dealId_imageId_cancellationToken, new object[] {dealId, imageId, cancellationToken}, () => impl.GetEstateImageAsync(dealId, imageId, cancellationToken));
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
            System.Reflection.MethodInfo mi_HandleBidderCreatedAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidderCreatedAsync(
                RA.Mspecs.Types.Event.EventBidderCreated e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleBidderCreatedAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleBidderCreatedAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleBidderUpdatedAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidderUpdatedAsync(
                RA.Mspecs.Types.Event.EventBidderUpdated e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleBidderUpdatedAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleBidderUpdatedAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleActivateAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleActivateAsync(
                RA.Mspecs.Types.Event.EventDealActivate e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleActivateAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleActivateAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleDeactivateAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleDeactivateAsync(
                RA.Mspecs.Types.Event.EventDealDeactivate e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleDeactivateAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleDeactivateAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleUpdateAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleUpdateAsync(
                RA.Mspecs.Types.Event.EventDealUpdate e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleUpdateAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleUpdateAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleBidUpdatedAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidUpdatedAsync(
                RA.Mspecs.Types.Event.EventBidUpdated e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleBidUpdatedAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleBidUpdatedAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleBiddingUpdatedAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBiddingUpdatedAsync(
                RA.Mspecs.Types.Event.EventBiddingUpdated e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleBiddingUpdatedAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleBiddingUpdatedAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleBiddingRestartAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBiddingRestartAsync(
                RA.Mspecs.Types.Event.EventBiddingRestart e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleBiddingRestartAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleBiddingRestartAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleBidCreatedAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidCreatedAsync(
                RA.Mspecs.Types.Event.EventBidCreated e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleBidCreatedAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleBidCreatedAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleBidWinningAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleBidWinningAsync(
                RA.Mspecs.Types.Event.EventBidWinning e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleBidWinningAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleBidWinningAsync(e, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_HandleSubscriberAddedAsync_e_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task HandleSubscriberAddedAsync(
                RA.Mspecs.Types.Event.EventSubscriberAdded e,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IEventHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_HandleSubscriberAddedAsync_e_cancellationToken, new object[] {e, cancellationToken}, () => impl.HandleSubscriberAddedAsync(e, cancellationToken));
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
            System.Reflection.MethodInfo mi_ProcessWebhookAsync_request_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ProcessWebhookAsync(
                RA.Mspecs.Types.HttpRequest request,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ProcessWebhookAsync_request_cancellationToken, new object[] {request, cancellationToken}, () => impl.ProcessWebhookAsync(request, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_InterpretWebhookAsync_uid_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task InterpretWebhookAsync(
                System.Guid uid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_InterpretWebhookAsync_uid_cancellationToken, new object[] {uid, cancellationToken}, () => impl.InterpretWebhookAsync(uid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_ListWebhookCallsAsync_fromDate_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.StoredHttpRequest>> ListWebhookCallsAsync(
                System.DateTimeOffset? fromDate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.StoredHttpRequest>>(_serviceProvider, impl, mi_ListWebhookCallsAsync_fromDate_cancellationToken, new object[] {fromDate, cancellationToken}, () => impl.ListWebhookCallsAsync(fromDate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_DownloadWebhookCallsAsync_fromDate_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fromDate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.FileContent> DownloadWebhookCallsAsync(
                System.DateTimeOffset? fromDate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.FileContent>(_serviceProvider, impl, mi_DownloadWebhookCallsAsync_fromDate_cancellationToken, new object[] {fromDate, cancellationToken}, () => impl.DownloadWebhookCallsAsync(fromDate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetServicesAsync_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Service.Service>> GetServicesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Service.Service>>(_serviceProvider, impl, mi_GetServicesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetServicesAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpdateServiceAsync_service_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="service"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateServiceAsync(
                RA.Mspecs.Types.Service.Service service,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateServiceAsync_service_cancellationToken, new object[] {service, cancellationToken}, () => impl.UpdateServiceAsync(service, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_SetWebhooksOnServicesAsync_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetWebhooksOnServicesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetWebhooksOnServicesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.SetWebhooksOnServicesAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetSubscribersAsync_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Subscriber.Subscriber>> GetSubscribersAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Subscriber.Subscriber>>(_serviceProvider, impl, mi_GetSubscribersAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetSubscribersAsync(cancellationToken));
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
            System.Reflection.MethodInfo mi_GetOfficeByIdAsync_id_useCache_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Office.Office>(_serviceProvider, impl, mi_GetOfficeByIdAsync_id_useCache_cancellationToken, new object[] {id, useCache, cancellationToken}, () => impl.GetOfficeByIdAsync(id, useCache, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpsertOfficeAsync_office_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="office"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.Office.Office> UpsertOfficeAsync(
                RA.Mspecs.Types.Office.Office office,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Office.Office>(_serviceProvider, impl, mi_UpsertOfficeAsync_office_cancellationToken, new object[] {office, cancellationToken}, () => impl.UpsertOfficeAsync(office, cancellationToken));
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
            System.Reflection.MethodInfo mi_RedirectBidderAsync_bidderId_cancellation;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="bidderId"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.FileContent> RedirectBidderAsync(
                System.String bidderId,
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Mspecs.Services.IRedirect)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.FileContent>(_serviceProvider, impl, mi_RedirectBidderAsync_bidderId_cancellation, new object[] {bidderId, cancellation}, () => impl.RedirectBidderAsync(bidderId, cancellation));
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
            System.Reflection.MethodInfo mi_GetCustomerEndpointsAsync_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Customer.CustomerEndpoint>> GetCustomerEndpointsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IUpdateHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Mspecs.Types.Customer.CustomerEndpoint>>(_serviceProvider, impl, mi_GetCustomerEndpointsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetCustomerEndpointsAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetCustomerEndpointAsync_subscriberId_useCache_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IUpdateHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.Customer.CustomerEndpoint>(_serviceProvider, impl, mi_GetCustomerEndpointAsync_subscriberId_useCache_cancellationToken, new object[] {subscriberId, useCache, cancellationToken}, () => impl.GetCustomerEndpointAsync(subscriberId, useCache, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpsertCustomerEndpointAsync_customerEndpoint_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="customerEndpoint"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertCustomerEndpointAsync(
                RA.Mspecs.Types.Customer.CustomerEndpoint customerEndpoint,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IUpdateHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertCustomerEndpointAsync_customerEndpoint_cancellationToken, new object[] {customerEndpoint, cancellationToken}, () => impl.UpsertCustomerEndpointAsync(customerEndpoint, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_StructureUpdatedAsync_dataType_id_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IUpdateHandler)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_StructureUpdatedAsync_dataType_id_cancellationToken, new object[] {dataType, id, cancellationToken}, () => impl.StructureUpdatedAsync(dataType, id, cancellationToken));
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
            System.Reflection.MethodInfo mi_GetUserByIdAsync_id_useCache_cancellationToken;
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
                var impl = (RA.Mspecs.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.User.User>(_serviceProvider, impl, mi_GetUserByIdAsync_id_useCache_cancellationToken, new object[] {id, useCache, cancellationToken}, () => impl.GetUserByIdAsync(id, useCache, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpsertUserAsync_user_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="user"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.User.User> UpsertUserAsync(
                RA.Mspecs.Types.User.User user,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.User.User>(_serviceProvider, impl, mi_UpsertUserAsync_user_cancellationToken, new object[] {user, cancellationToken}, () => impl.UpsertUserAsync(user, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetUserImageAsync_userId_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Mspecs.Types.FileContent> GetUserImageAsync(
                System.String userId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Mspecs.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Mspecs.Types.FileContent>(_serviceProvider, impl, mi_GetUserImageAsync_userId_cancellationToken, new object[] {userId, cancellationToken}, () => impl.GetUserImageAsync(userId, cancellationToken));
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