namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// 
    /// </summary>
    public static class RACustomerExtensions {
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesIBoostadAdminProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.IBoostadAdmin>,RA.Customer.Services.IBoostadAdmin {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesIBoostadAdminProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.IBoostadAdmin> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateByUidAsync_estateUid_mustExist_cancellationToken = GetMethodInfo("GetEstateByUidAsync", new System.Type[] {typeof(System.Nullable<System.Guid>), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="mustExist"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> GetEstateByUidAsync(
                System.Guid? estateUid,
                System.Boolean mustExist,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_GetEstateByUidAsync_estateUid_mustExist_cancellationToken, new object[] {estateUid, mustExist, cancellationToken}, () => impl.GetEstateByUidAsync(estateUid, mustExist, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesForBrokerAsync_brokerUid_cancellationToken = GetMethodInfo("GetEstatesForBrokerAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="brokerUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>> GetEstatesForBrokerAsync(
                System.Guid brokerUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>>(_serviceProvider, impl, mi_GetEstatesForBrokerAsync_brokerUid_cancellationToken, new object[] {brokerUid, cancellationToken}, () => impl.GetEstatesForBrokerAsync(brokerUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesForBrokerByExternalIdAsync_brokerExternalId_cancellationToken = GetMethodInfo("GetEstatesForBrokerByExternalIdAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="brokerExternalId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>> GetEstatesForBrokerByExternalIdAsync(
                RA.Customer.Types.Backend.ExternalID brokerExternalId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>>(_serviceProvider, impl, mi_GetEstatesForBrokerByExternalIdAsync_brokerExternalId_cancellationToken, new object[] {brokerExternalId, cancellationToken}, () => impl.GetEstatesForBrokerByExternalIdAsync(brokerExternalId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateByExternalIdAsync_externalId_cancellationToken = GetMethodInfo("GetEstateByExternalIdAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> GetEstateByExternalIdAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_GetEstateByExternalIdAsync_externalId_cancellationToken, new object[] {externalId, cancellationToken}, () => impl.GetEstateByExternalIdAsync(externalId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactByUidAsync_contactUid_cancellationToken = GetMethodInfo("GetContactByUidAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> GetContactByUidAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_GetContactByUidAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.GetContactByUidAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_MakeDocumentAvailableInStepsAsync_estateUid_documentUid_view_steps_cancellationToken = GetMethodInfo("MakeDocumentAvailableInStepsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.String), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="documentUid"></param>
            /// <param name="view"></param>
            /// <param name="steps"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task MakeDocumentAvailableInStepsAsync(
                System.Guid estateUid,
                System.Guid documentUid,
                System.String view,
                System.Collections.Generic.IEnumerable<System.String> steps,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_MakeDocumentAvailableInStepsAsync_estateUid_documentUid_view_steps_cancellationToken, new object[] {estateUid, documentUid, view, steps, cancellationToken}, () => impl.MakeDocumentAvailableInStepsAsync(estateUid, documentUid, view, steps, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateEstateSettingsAsync_estateUid_settings_cancellationToken = GetMethodInfo("UpdateEstateSettingsAsync", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.Backend.Estate.EstateSettings), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="settings"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateEstateSettingsAsync(
                System.Guid estateUid,
                RA.Customer.Types.Backend.Estate.EstateSettings settings,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateEstateSettingsAsync_estateUid_settings_cancellationToken, new object[] {estateUid, settings, cancellationToken}, () => impl.UpdateEstateSettingsAsync(estateUid, settings, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ImpersonateContactByUidAsync_contactUid_cancellationToken = GetMethodInfo("ImpersonateContactByUidAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> ImpersonateContactByUidAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_ImpersonateContactByUidAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.ImpersonateContactByUidAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactEstateLinkAsync_source_contactUid_view_estateUid_cancellationToken = GetMethodInfo("GetContactEstateLinkAsync", new System.Type[] {typeof(System.String), typeof(System.Guid), typeof(System.String), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="contactUid"></param>
            /// <param name="view"></param>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> GetContactEstateLinkAsync(
                System.String source,
                System.Guid contactUid,
                System.String view,
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IBoostadAdmin)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetContactEstateLinkAsync_source_contactUid_view_estateUid_cancellationToken, new object[] {source, contactUid, view, estateUid, cancellationToken}, () => impl.GetContactEstateLinkAsync(source, contactUid, view, estateUid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesIImpersonateProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.IImpersonate>,RA.Customer.Services.IImpersonate {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesIImpersonateProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.IImpersonate> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ImpersonateUserAsync_ssn_cancellationToken = GetMethodInfo("ImpersonateUserAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ssn"></param>
            /// <param name="cancellationToken"></param>
            [System.Obsolete]
            public System.Threading.Tasks.Task<System.String> ImpersonateUserAsync(
                System.String ssn,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IImpersonate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_ImpersonateUserAsync_ssn_cancellationToken, new object[] {ssn, cancellationToken}, () => impl.ImpersonateUserAsync(ssn, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ImpersonateUserWithRedirectAsync_ssn_redirectUrl_cancellationToken = GetMethodInfo("ImpersonateUserWithRedirectAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ssn"></param>
            /// <param name="redirectUrl"></param>
            /// <param name="cancellationToken"></param>
            [System.Obsolete]
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> ImpersonateUserWithRedirectAsync(
                System.String ssn,
                System.String redirectUrl,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IImpersonate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_ImpersonateUserWithRedirectAsync_ssn_redirectUrl_cancellationToken, new object[] {ssn, redirectUrl, cancellationToken}, () => impl.ImpersonateUserWithRedirectAsync(ssn, redirectUrl, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ImpersonateContactByUidAsync_contactUid_cancellationToken = GetMethodInfo("ImpersonateContactByUidAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> ImpersonateContactByUidAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IImpersonate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_ImpersonateContactByUidAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.ImpersonateContactByUidAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ImpersonateContactByUidWithRedirectAsync_contactUid_view_estateUid_cancellationToken = GetMethodInfo("ImpersonateContactByUidWithRedirectAsync", new System.Type[] {typeof(System.Guid), typeof(System.String), typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="view"></param>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> ImpersonateContactByUidWithRedirectAsync(
                System.Guid contactUid,
                System.String view,
                System.Guid? estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IImpersonate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_ImpersonateContactByUidWithRedirectAsync_contactUid_view_estateUid_cancellationToken, new object[] {contactUid, view, estateUid, cancellationToken}, () => impl.ImpersonateContactByUidWithRedirectAsync(contactUid, view, estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ImpersonateContactByExternalIdWithRedirectAsync_source_contactId_view_estateId_cancellationToken = GetMethodInfo("ImpersonateContactByExternalIdWithRedirectAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="contactId"></param>
            /// <param name="view"></param>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> ImpersonateContactByExternalIdWithRedirectAsync(
                System.String source,
                System.String contactId,
                System.String view,
                System.String estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.IImpersonate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_ImpersonateContactByExternalIdWithRedirectAsync_source_contactId_view_estateId_cancellationToken, new object[] {source, contactId, view, estateId, cancellationToken}, () => impl.ImpersonateContactByExternalIdWithRedirectAsync(source, contactId, view, estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesISettingsProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.ISettings>,RA.Customer.Services.ISettings {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesISettingsProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.ISettings> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateSettingsFromExcelAsync_excelFile_cancellationToken = GetMethodInfo("UpdateSettingsFromExcelAsync", new System.Type[] {typeof(RA.Customer.Types.FileContent), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="excelFile"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Settings.SettingsStatus> UpdateSettingsFromExcelAsync(
                RA.Customer.Types.FileContent excelFile,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.ISettings)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Settings.SettingsStatus>(_serviceProvider, impl, mi_UpdateSettingsFromExcelAsync_excelFile_cancellationToken, new object[] {excelFile, cancellationToken}, () => impl.UpdateSettingsFromExcelAsync(excelFile, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateSettingsExcelAsync_cancellationToken = GetMethodInfo("CreateSettingsExcelAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> CreateSettingsExcelAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.ISettings)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_CreateSettingsExcelAsync_cancellationToken, new object[] {cancellationToken}, () => impl.CreateSettingsExcelAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ListSettingsAsync_cancellationToken = GetMethodInfo("ListSettingsAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Settings.SettingsStatus>> ListSettingsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.ISettings)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Settings.SettingsStatus>>(_serviceProvider, impl, mi_ListSettingsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.ListSettingsAsync(cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesPublicIApplicationProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Public.IApplication>,RA.Customer.Services.Public.IApplication {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesPublicIApplicationProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Public.IApplication> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetIndexHtmlAsync_cancellationToken = GetMethodInfo("GetIndexHtmlAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetIndexHtmlAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IApplication)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetIndexHtmlAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetIndexHtmlAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateLinkAsync_source_view_estateUid_cancellationToken = GetMethodInfo("GetEstateLinkAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="view"></param>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> GetEstateLinkAsync(
                System.String source,
                System.String view,
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IApplication)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetEstateLinkAsync_source_view_estateUid_cancellationToken, new object[] {source, view, estateUid, cancellationToken}, () => impl.GetEstateLinkAsync(source, view, estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactEstateLinkAsync_source_contactUid_view_estateUid_cancellation = GetMethodInfo("GetContactEstateLinkAsync", new System.Type[] {typeof(System.String), typeof(System.Guid), typeof(System.String), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="contactUid"></param>
            /// <param name="view"></param>
            /// <param name="estateUid"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Uri> GetContactEstateLinkAsync(
                System.String source,
                System.Guid contactUid,
                System.String view,
                System.Guid estateUid,
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Public.IApplication)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetContactEstateLinkAsync_source_contactUid_view_estateUid_cancellation, new object[] {source, contactUid, view, estateUid, cancellation}, () => impl.GetContactEstateLinkAsync(source, contactUid, view, estateUid, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactEstateViewAsync_source_contactId_view_estateId_cancellationToken = GetMethodInfo("GetContactEstateViewAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="contactId"></param>
            /// <param name="view"></param>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetContactEstateViewAsync(
                System.String source,
                System.String contactId,
                System.String view,
                System.String estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IApplication)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetContactEstateViewAsync_source_contactId_view_estateId_cancellationToken, new object[] {source, contactId, view, estateId, cancellationToken}, () => impl.GetContactEstateViewAsync(source, contactId, view, estateId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetApplicationDataAsync_cancellation = GetMethodInfo("GetApplicationDataAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.ApplicationData> GetApplicationDataAsync(
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Public.IApplication)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.ApplicationData>(_serviceProvider, impl, mi_GetApplicationDataAsync_cancellation, new object[] {cancellation}, () => impl.GetApplicationDataAsync(cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSteps_cancellation = GetMethodInfo("GetSteps", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.EstateStep>> GetSteps(
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Public.IApplication)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.EstateStep>>(_serviceProvider, impl, mi_GetSteps_cancellation, new object[] {cancellation}, () => impl.GetSteps(cancellation));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesPublicIBankIdProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Public.IBankId>,RA.Customer.Services.Public.IBankId {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesPublicIBankIdProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Public.IBankId> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetClientIpAsync_cancellationToken = GetMethodInfo("GetClientIpAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> GetClientIpAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_GetClientIpAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetClientIpAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateOidcScopeAsync_redirectUri_userAgent_cancellationToken = GetMethodInfo("CreateOidcScopeAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="redirectUri"></param>
            /// <param name="userAgent"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> CreateOidcScopeAsync(
                System.String redirectUri,
                System.String userAgent,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_CreateOidcScopeAsync_redirectUri_userAgent_cancellationToken, new object[] {redirectUri, userAgent, cancellationToken}, () => impl.CreateOidcScopeAsync(redirectUri, userAgent, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_StartLocalLoginAsync_oidcScopeId_clientIp_cancellationToken = GetMethodInfo("StartLocalLoginAsync", new System.Type[] {typeof(System.Guid), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="oidcScopeId"></param>
            /// <param name="clientIp"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> StartLocalLoginAsync(
                System.Guid oidcScopeId,
                System.String clientIp,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_StartLocalLoginAsync_oidcScopeId_clientIp_cancellationToken, new object[] {oidcScopeId, clientIp, cancellationToken}, () => impl.StartLocalLoginAsync(oidcScopeId, clientIp, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_StartLoginAsync_oidcScopeId_ssn_clientIp_cancellationToken = GetMethodInfo("StartLoginAsync", new System.Type[] {typeof(System.Guid), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="oidcScopeId"></param>
            /// <param name="ssn"></param>
            /// <param name="clientIp"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BankId.AuthResponse> StartLoginAsync(
                System.Guid oidcScopeId,
                System.String ssn,
                System.String clientIp,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BankId.AuthResponse>(_serviceProvider, impl, mi_StartLoginAsync_oidcScopeId_ssn_clientIp_cancellationToken, new object[] {oidcScopeId, ssn, clientIp, cancellationToken}, () => impl.StartLoginAsync(oidcScopeId, ssn, clientIp, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateQrCodeAsync_autoStartToken_cancellationToken = GetMethodInfo("CreateQrCodeAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="autoStartToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> CreateQrCodeAsync(
                System.String autoStartToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_CreateQrCodeAsync_autoStartToken_cancellationToken, new object[] {autoStartToken, cancellationToken}, () => impl.CreateQrCodeAsync(autoStartToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetAuthStatusAsync_oidcScopeId_cancellationToken = GetMethodInfo("GetAuthStatusAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="oidcScopeId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BankId.AuthStatus> GetAuthStatusAsync(
                System.Guid oidcScopeId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BankId.AuthStatus>(_serviceProvider, impl, mi_GetAuthStatusAsync_oidcScopeId_cancellationToken, new object[] {oidcScopeId, cancellationToken}, () => impl.GetAuthStatusAsync(oidcScopeId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetJwtTokenAsync_oidcScopeId_cancellationToken = GetMethodInfo("GetJwtTokenAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="oidcScopeId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> GetJwtTokenAsync(
                System.Guid oidcScopeId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_GetJwtTokenAsync_oidcScopeId_cancellationToken, new object[] {oidcScopeId, cancellationToken}, () => impl.GetJwtTokenAsync(oidcScopeId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetRedirectAsync_oidcScopeId_cancellationToken = GetMethodInfo("GetRedirectAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="oidcScopeId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> GetRedirectAsync(
                System.Guid oidcScopeId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_GetRedirectAsync_oidcScopeId_cancellationToken, new object[] {oidcScopeId, cancellationToken}, () => impl.GetRedirectAsync(oidcScopeId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RefreshTokenAsync_jwt_cancellationToken = GetMethodInfo("RefreshTokenAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="jwt"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> RefreshTokenAsync(
                System.String jwt,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Public.IBankId)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_RefreshTokenAsync_jwt_cancellationToken, new object[] {jwt, cancellationToken}, () => impl.RefreshTokenAsync(jwt, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIAdvertiseProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IAdvertise>,RA.Customer.Services.Backend.IAdvertise {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIAdvertiseProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IAdvertise> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetAdvertsAsync_cancellationToken = GetMethodInfo("GetAdvertsAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Advertise.Advert>> GetAdvertsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IAdvertise)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Advertise.Advert>>(_serviceProvider, impl, mi_GetAdvertsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetAdvertsAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertAdvertAsync_promotion_cancellationToken = GetMethodInfo("UpsertAdvertAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Advertise.Advert), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="promotion"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Boolean> UpsertAdvertAsync(
                RA.Customer.Types.Backend.Advertise.Advert promotion,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IAdvertise)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Boolean>(_serviceProvider, impl, mi_UpsertAdvertAsync_promotion_cancellationToken, new object[] {promotion, cancellationToken}, () => impl.UpsertAdvertAsync(promotion, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DeleteAsync_uid_cancellationToken = GetMethodInfo("DeleteAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteAsync(
                System.Guid uid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IAdvertise)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DeleteAsync_uid_cancellationToken, new object[] {uid, cancellationToken}, () => impl.DeleteAsync(uid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIBrokerProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IBroker>,RA.Customer.Services.Backend.IBroker {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIBrokerProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IBroker> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ListAllBrokersAsync_useCache_cancellationToken = GetMethodInfo("ListAllBrokersAsync", new System.Type[] {typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="useCache"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Broker.Broker>> ListAllBrokersAsync(
                System.Boolean useCache,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Broker.Broker>>(_serviceProvider, impl, mi_ListAllBrokersAsync_useCache_cancellationToken, new object[] {useCache, cancellationToken}, () => impl.ListAllBrokersAsync(useCache, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckBrokersAsync_cancellationToken = GetMethodInfo("CheckBrokersAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckBrokersAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckBrokersAsync_cancellationToken, new object[] {cancellationToken}, () => impl.CheckBrokersAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckBrokerAsync_brokerUid_cancellationToken = GetMethodInfo("CheckBrokerAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="brokerUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckBrokerAsync(
                System.Guid brokerUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckBrokerAsync_brokerUid_cancellationToken, new object[] {brokerUid, cancellationToken}, () => impl.CheckBrokerAsync(brokerUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBrokerByUidAsync_uid_cancellationToken = GetMethodInfo("GetBrokerByUidAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Broker.Broker> GetBrokerByUidAsync(
                System.Guid uid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Broker.Broker>(_serviceProvider, impl, mi_GetBrokerByUidAsync_uid_cancellationToken, new object[] {uid, cancellationToken}, () => impl.GetBrokerByUidAsync(uid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBrokerByExternalIdAsync_externalId_mustExist_fetchMissingBroker_cancellationToken = GetMethodInfo("GetBrokerByExternalIdAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="mustExist"></param>
            /// <param name="fetchMissingBroker"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Broker.Broker> GetBrokerByExternalIdAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Boolean mustExist,
                System.Boolean fetchMissingBroker,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Broker.Broker>(_serviceProvider, impl, mi_GetBrokerByExternalIdAsync_externalId_mustExist_fetchMissingBroker_cancellationToken, new object[] {externalId, mustExist, fetchMissingBroker, cancellationToken}, () => impl.GetBrokerByExternalIdAsync(externalId, mustExist, fetchMissingBroker, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertBrokerAsync_broker_cancellationToken = GetMethodInfo("UpsertBrokerAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Broker.Broker), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="broker"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Broker.Broker> UpsertBrokerAsync(
                RA.Customer.Types.Backend.Broker.Broker broker,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Broker.Broker>(_serviceProvider, impl, mi_UpsertBrokerAsync_broker_cancellationToken, new object[] {broker, cancellationToken}, () => impl.UpsertBrokerAsync(broker, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBrokerImageAsync_brokerUid_cancellationToken = GetMethodInfo("GetBrokerImageAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="brokerUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetBrokerImageAsync(
                System.Guid brokerUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetBrokerImageAsync_brokerUid_cancellationToken, new object[] {brokerUid, cancellationToken}, () => impl.GetBrokerImageAsync(brokerUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertBrokerImageAsync_data_cancellationToken = GetMethodInfo("UpsertBrokerImageAsync", new System.Type[] {typeof(RA.Customer.Types.FileContent), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="data"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertBrokerImageAsync(
                RA.Customer.Types.FileContent data,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertBrokerImageAsync_data_cancellationToken, new object[] {data, cancellationToken}, () => impl.UpsertBrokerImageAsync(data, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckBrokerImageAsync_uid_cancellationToken = GetMethodInfo("CheckBrokerImageAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckBrokerImageAsync(
                System.Guid uid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckBrokerImageAsync_uid_cancellationToken, new object[] {uid, cancellationToken}, () => impl.CheckBrokerImageAsync(uid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateNewBrokerAsync_externalId_cancellationToken = GetMethodInfo("CreateNewBrokerAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Broker.Broker> CreateNewBrokerAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IBroker)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Broker.Broker>(_serviceProvider, impl, mi_CreateNewBrokerAsync_externalId_cancellationToken, new object[] {externalId, cancellationToken}, () => impl.CreateNewBrokerAsync(externalId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIChecklistProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IChecklist>,RA.Customer.Services.Backend.IChecklist {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIChecklistProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IChecklist> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ListChecklistsAsync_view_step_estateUid_cancellationToken = GetMethodInfo("ListChecklistsAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="view"></param>
            /// <param name="step"></param>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Checklist.Checklist>> ListChecklistsAsync(
                System.String view,
                System.String step,
                System.Guid? estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IChecklist)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Checklist.Checklist>>(_serviceProvider, impl, mi_ListChecklistsAsync_view_step_estateUid_cancellationToken, new object[] {view, step, estateUid, cancellationToken}, () => impl.ListChecklistsAsync(view, step, estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetChecklistAsync_checklistUid_estateUid_cancellationToken = GetMethodInfo("GetChecklistAsync", new System.Type[] {typeof(System.Guid), typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="checklistUid"></param>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Checklist.Checklist> GetChecklistAsync(
                System.Guid checklistUid,
                System.Guid? estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IChecklist)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Checklist.Checklist>(_serviceProvider, impl, mi_GetChecklistAsync_checklistUid_estateUid_cancellationToken, new object[] {checklistUid, estateUid, cancellationToken}, () => impl.GetChecklistAsync(checklistUid, estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_StoreChecklistsAsync_lists_cancellationToken = GetMethodInfo("StoreChecklistsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Checklist.Checklist>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lists"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task StoreChecklistsAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Checklist.Checklist> lists,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IChecklist)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_StoreChecklistsAsync_lists_cancellationToken, new object[] {lists, cancellationToken}, () => impl.StoreChecklistsAsync(lists, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ListChecklistEstateItemsAsync_estateUid_listUid_cancellationToken = GetMethodInfo("ListChecklistEstateItemsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="listUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Checklist.ChecklistEstateItem>> ListChecklistEstateItemsAsync(
                System.Guid estateUid,
                System.Guid? listUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IChecklist)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Checklist.ChecklistEstateItem>>(_serviceProvider, impl, mi_ListChecklistEstateItemsAsync_estateUid_listUid_cancellationToken, new object[] {estateUid, listUid, cancellationToken}, () => impl.ListChecklistEstateItemsAsync(estateUid, listUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SetChecklistEstateItemAsync_todoEstateItem_cancellationToken = GetMethodInfo("SetChecklistEstateItemAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Checklist.ChecklistEstateItem), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="todoEstateItem"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetChecklistEstateItemAsync(
                RA.Customer.Types.Backend.Checklist.ChecklistEstateItem todoEstateItem,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IChecklist)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetChecklistEstateItemAsync_todoEstateItem_cancellationToken, new object[] {todoEstateItem, cancellationToken}, () => impl.SetChecklistEstateItemAsync(todoEstateItem, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AddCheckListEstateItemAsync_estateUid_checkListUid_checklistItem_cancellationToken = GetMethodInfo("AddCheckListEstateItemAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(RA.Customer.Types.Backend.Checklist.ChecklistItem), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="checkListUid"></param>
            /// <param name="checklistItem"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task AddCheckListEstateItemAsync(
                System.Guid estateUid,
                System.Guid checkListUid,
                RA.Customer.Types.Backend.Checklist.ChecklistItem checklistItem,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IChecklist)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_AddCheckListEstateItemAsync_estateUid_checkListUid_checklistItem_cancellationToken, new object[] {estateUid, checkListUid, checklistItem, cancellationToken}, () => impl.AddCheckListEstateItemAsync(estateUid, checkListUid, checklistItem, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIContactProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IContact>,RA.Customer.Services.Backend.IContact {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIContactProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IContact> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactUpdateDateTimeAsync_contactUid_cancellationToken = GetMethodInfo("GetContactUpdateDateTimeAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.ContactUpdateDateTime> GetContactUpdateDateTimeAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.ContactUpdateDateTime>(_serviceProvider, impl, mi_GetContactUpdateDateTimeAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.GetContactUpdateDateTimeAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertContactUpdateDateTimeAsync_cudt_cancellationToken = GetMethodInfo("UpsertContactUpdateDateTimeAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Contact.ContactUpdateDateTime), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cudt"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertContactUpdateDateTimeAsync(
                RA.Customer.Types.Backend.Contact.ContactUpdateDateTime cudt,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertContactUpdateDateTimeAsync_cudt_cancellationToken, new object[] {cudt, cancellationToken}, () => impl.UpsertContactUpdateDateTimeAsync(cudt, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactBySsnAsync_ssn_cancellationToken = GetMethodInfo("GetContactBySsnAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ssn"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> GetContactBySsnAsync(
                System.String ssn,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_GetContactBySsnAsync_ssn_cancellationToken, new object[] {ssn, cancellationToken}, () => impl.GetContactBySsnAsync(ssn, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactByUidAsync_contactUid_mustExist_fetchMergedContact_cancellationToken = GetMethodInfo("GetContactByUidAsync", new System.Type[] {typeof(System.Guid), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="mustExist"></param>
            /// <param name="fetchMergedContact"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> GetContactByUidAsync(
                System.Guid contactUid,
                System.Boolean mustExist,
                System.Boolean fetchMergedContact,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_GetContactByUidAsync_contactUid_mustExist_fetchMergedContact_cancellationToken, new object[] {contactUid, mustExist, fetchMergedContact, cancellationToken}, () => impl.GetContactByUidAsync(contactUid, mustExist, fetchMergedContact, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactByExternalIdAsync_externalID_mustExist_fetchMissingContact_cancellationToken = GetMethodInfo("GetContactByExternalIdAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalID"></param>
            /// <param name="mustExist"></param>
            /// <param name="fetchMissingContact"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> GetContactByExternalIdAsync(
                RA.Customer.Types.Backend.ExternalID externalID,
                System.Boolean mustExist,
                System.Boolean fetchMissingContact,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_GetContactByExternalIdAsync_externalID_mustExist_fetchMissingContact_cancellationToken, new object[] {externalID, mustExist, fetchMissingContact, cancellationToken}, () => impl.GetContactByExternalIdAsync(externalID, mustExist, fetchMissingContact, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactsByExternalIdsAsync_externalIds_fetchMissingContact_cancellationToken = GetMethodInfo("GetContactsByExternalIdsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.ExternalID>), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalIds"></param>
            /// <param name="fetchMissingContact"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Contact.Contact>> GetContactsByExternalIdsAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.ExternalID> externalIds,
                System.Boolean fetchMissingContact,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Contact.Contact>>(_serviceProvider, impl, mi_GetContactsByExternalIdsAsync_externalIds_fetchMissingContact_cancellationToken, new object[] {externalIds, fetchMissingContact, cancellationToken}, () => impl.GetContactsByExternalIdsAsync(externalIds, fetchMissingContact, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactUidsByExternalIdsAsync_externalIds_fetchMissingContact_cancellationToken = GetMethodInfo("GetContactUidsByExternalIdsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.ExternalID>), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalIds"></param>
            /// <param name="fetchMissingContact"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.Guid>> GetContactUidsByExternalIdsAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.ExternalID> externalIds,
                System.Boolean fetchMissingContact,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<System.Guid>>(_serviceProvider, impl, mi_GetContactUidsByExternalIdsAsync_externalIds_fetchMissingContact_cancellationToken, new object[] {externalIds, fetchMissingContact, cancellationToken}, () => impl.GetContactUidsByExternalIdsAsync(externalIds, fetchMissingContact, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactsByUidsAsync_contactIds_cancellationToken = GetMethodInfo("GetContactsByUidsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactIds"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Contact.Contact>> GetContactsByUidsAsync(
                System.Collections.Generic.IEnumerable<System.Guid> contactIds,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Contact.Contact>>(_serviceProvider, impl, mi_GetContactsByUidsAsync_contactIds_cancellationToken, new object[] {contactIds, cancellationToken}, () => impl.GetContactsByUidsAsync(contactIds, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SyncWithBrokerSystemAsync_contactUid_cancellationToken = GetMethodInfo("SyncWithBrokerSystemAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> SyncWithBrokerSystemAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_SyncWithBrokerSystemAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.SyncWithBrokerSystemAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SyncWithBrokerSystemDetachedAsync_contactUid_cancellationToken = GetMethodInfo("SyncWithBrokerSystemDetachedAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SyncWithBrokerSystemDetachedAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SyncWithBrokerSystemDetachedAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.SyncWithBrokerSystemDetachedAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactsAsync_continuationToken_cancellationToken = GetMethodInfo("GetContactsAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="continuationToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.ContactList> GetContactsAsync(
                System.String continuationToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.ContactList>(_serviceProvider, impl, mi_GetContactsAsync_continuationToken_cancellationToken, new object[] {continuationToken, cancellationToken}, () => impl.GetContactsAsync(continuationToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertContactAsync_contact_cancellationToken = GetMethodInfo("UpsertContactAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Contact.Contact), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contact"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> UpsertContactAsync(
                RA.Customer.Types.Backend.Contact.Contact contact,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_UpsertContactAsync_contact_cancellationToken, new object[] {contact, cancellationToken}, () => impl.UpsertContactAsync(contact, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertContactAsync_contact_checkForDuplicates_cancellationToken = GetMethodInfo("UpsertContactAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Contact.Contact), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contact"></param>
            /// <param name="checkForDuplicates"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> UpsertContactAsync(
                RA.Customer.Types.Backend.Contact.Contact contact,
                System.Boolean checkForDuplicates,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_UpsertContactAsync_contact_checkForDuplicates_cancellationToken, new object[] {contact, checkForDuplicates, cancellationToken}, () => impl.UpsertContactAsync(contact, checkForDuplicates, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DeleteContactAsync_contactUid_cancellationToken = GetMethodInfo("DeleteContactAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteContactAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DeleteContactAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.DeleteContactAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_MergeContactsAsync_contactUid1_contactUid2_cancellationToken = GetMethodInfo("MergeContactsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid1"></param>
            /// <param name="contactUid2"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> MergeContactsAsync(
                System.Guid contactUid1,
                System.Guid contactUid2,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_MergeContactsAsync_contactUid1_contactUid2_cancellationToken, new object[] {contactUid1, contactUid2, cancellationToken}, () => impl.MergeContactsAsync(contactUid1, contactUid2, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_PostContactMergeAsync_removedContactUid_cancellationToken = GetMethodInfo("PostContactMergeAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="removedContactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task PostContactMergeAsync(
                System.Guid removedContactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_PostContactMergeAsync_removedContactUid_cancellationToken, new object[] {removedContactUid, cancellationToken}, () => impl.PostContactMergeAsync(removedContactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SplitContactAsync_contactUid_externalId_cancellationToken = GetMethodInfo("SplitContactAsync", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="externalId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> SplitContactAsync(
                System.Guid contactUid,
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_SplitContactAsync_contactUid_externalId_cancellationToken, new object[] {contactUid, externalId, cancellationToken}, () => impl.SplitContactAsync(contactUid, externalId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_PostContactSplitAsync_newContactUid_cancellationToken = GetMethodInfo("PostContactSplitAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="newContactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task PostContactSplitAsync(
                System.Guid newContactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_PostContactSplitAsync_newContactUid_cancellationToken, new object[] {newContactUid, cancellationToken}, () => impl.PostContactSplitAsync(newContactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckContactsAsync_continuationToken_cancellationToken = GetMethodInfo("CheckContactsAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="continuationToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckContactsAsync(
                System.String continuationToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckContactsAsync_continuationToken_cancellationToken, new object[] {continuationToken, cancellationToken}, () => impl.CheckContactsAsync(continuationToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckContactAsync_contactUid_cancellationToken = GetMethodInfo("CheckContactAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckContactAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckContactAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.CheckContactAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateNewContactAsync_externalId_name_cancellationToken = GetMethodInfo("CreateNewContactAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="name"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> CreateNewContactAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.String name,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IContact)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_CreateNewContactAsync_externalId_name_cancellationToken, new object[] {externalId, name, cancellationToken}, () => impl.CreateNewContactAsync(externalId, name, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIEstateProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IEstate>,RA.Customer.Services.Backend.IEstate {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIEstateProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IEstate> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetExternalEstateLinkAsync_guid_cancellation = GetMethodInfo("GetExternalEstateLinkAsync", new System.Type[] {typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="guid"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Uri> GetExternalEstateLinkAsync(
                System.Guid? guid,
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetExternalEstateLinkAsync_guid_cancellation, new object[] {guid, cancellation}, () => impl.GetExternalEstateLinkAsync(guid, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateByUidAsync_estateUid_mustExist_cancellationToken = GetMethodInfo("GetEstateByUidAsync", new System.Type[] {typeof(System.Nullable<System.Guid>), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="mustExist"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> GetEstateByUidAsync(
                System.Guid? estateUid,
                System.Boolean mustExist,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_GetEstateByUidAsync_estateUid_mustExist_cancellationToken, new object[] {estateUid, mustExist, cancellationToken}, () => impl.GetEstateByUidAsync(estateUid, mustExist, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateByExternalIdAsync_externalId_mustExist_fetchMissingEstate_cancellationToken = GetMethodInfo("GetEstateByExternalIdAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="mustExist"></param>
            /// <param name="fetchMissingEstate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> GetEstateByExternalIdAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Boolean mustExist,
                System.Boolean fetchMissingEstate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_GetEstateByExternalIdAsync_externalId_mustExist_fetchMissingEstate_cancellationToken, new object[] {externalId, mustExist, fetchMissingEstate, cancellationToken}, () => impl.GetEstateByExternalIdAsync(externalId, mustExist, fetchMissingEstate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DetachEstateAndChangeBuyerAndSeller_estateUid_buyerAndSellerUid_cancellationToken = GetMethodInfo("DetachEstateAndChangeBuyerAndSeller", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="buyerAndSellerUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DetachEstateAndChangeBuyerAndSeller(
                System.Guid estateUid,
                System.Guid buyerAndSellerUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DetachEstateAndChangeBuyerAndSeller_estateUid_buyerAndSellerUid_cancellationToken, new object[] {estateUid, buyerAndSellerUid, cancellationToken}, () => impl.DetachEstateAndChangeBuyerAndSeller(estateUid, buyerAndSellerUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesForContactAsync_contactUid_cancellationToken = GetMethodInfo("GetEstatesForContactAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>> GetEstatesForContactAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>>(_serviceProvider, impl, mi_GetEstatesForContactAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.GetEstatesForContactAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesForBrokerAsync_brokerUid_cancellationToken = GetMethodInfo("GetEstatesForBrokerAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="brokerUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>> GetEstatesForBrokerAsync(
                System.Guid brokerUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>>(_serviceProvider, impl, mi_GetEstatesForBrokerAsync_brokerUid_cancellationToken, new object[] {brokerUid, cancellationToken}, () => impl.GetEstatesForBrokerAsync(brokerUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesForSellerBySsnAsync_ssn_cancellationToken = GetMethodInfo("GetEstatesForSellerBySsnAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ssn"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>> GetEstatesForSellerBySsnAsync(
                System.String ssn,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>>(_serviceProvider, impl, mi_GetEstatesForSellerBySsnAsync_ssn_cancellationToken, new object[] {ssn, cancellationToken}, () => impl.GetEstatesForSellerBySsnAsync(ssn, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesForBuyerBySsnAsync_ssn_cancellationToken = GetMethodInfo("GetEstatesForBuyerBySsnAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ssn"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>> GetEstatesForBuyerBySsnAsync(
                System.String ssn,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>>(_serviceProvider, impl, mi_GetEstatesForBuyerBySsnAsync_ssn_cancellationToken, new object[] {ssn, cancellationToken}, () => impl.GetEstatesForBuyerBySsnAsync(ssn, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesForBidderParticipantBySsnAsync_ssn_cancellationToken = GetMethodInfo("GetEstatesForBidderParticipantBySsnAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ssn"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>> GetEstatesForBidderParticipantBySsnAsync(
                System.String ssn,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.Estate>>(_serviceProvider, impl, mi_GetEstatesForBidderParticipantBySsnAsync_ssn_cancellationToken, new object[] {ssn, cancellationToken}, () => impl.GetEstatesForBidderParticipantBySsnAsync(ssn, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstatesAsync_continuationToken_cancellationToken = GetMethodInfo("GetEstatesAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="continuationToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.EstateList> GetEstatesAsync(
                System.String continuationToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.EstateList>(_serviceProvider, impl, mi_GetEstatesAsync_continuationToken_cancellationToken, new object[] {continuationToken, cancellationToken}, () => impl.GetEstatesAsync(continuationToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckEstatesAsync_continuationToken_cancellationToken = GetMethodInfo("CheckEstatesAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="continuationToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckEstatesAsync(
                System.String continuationToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckEstatesAsync_continuationToken_cancellationToken, new object[] {continuationToken, cancellationToken}, () => impl.CheckEstatesAsync(continuationToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckEstateAsync_estateUid_cancellationToken = GetMethodInfo("CheckEstateAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckEstateAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckEstateAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.CheckEstateAsync(estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertEstateAsync_estate_cancellationToken = GetMethodInfo("UpsertEstateAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Estate.Estate), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> UpsertEstateAsync(
                RA.Customer.Types.Backend.Estate.Estate estate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_UpsertEstateAsync_estate_cancellationToken, new object[] {estate, cancellationToken}, () => impl.UpsertEstateAsync(estate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertEstateAsync_estate_checkReferences_cancellationToken = GetMethodInfo("UpsertEstateAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Estate.Estate), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estate"></param>
            /// <param name="checkReferences"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> UpsertEstateAsync(
                RA.Customer.Types.Backend.Estate.Estate estate,
                System.Boolean checkReferences,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_UpsertEstateAsync_estate_checkReferences_cancellationToken, new object[] {estate, checkReferences, cancellationToken}, () => impl.UpsertEstateAsync(estate, checkReferences, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DeleteEstateAsync_estateUid_cancellationToken = GetMethodInfo("DeleteEstateAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteEstateAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DeleteEstateAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.DeleteEstateAsync(estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateEstateSettingsAsync_estateUid_settings_cancellationToken = GetMethodInfo("UpdateEstateSettingsAsync", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.Backend.Estate.EstateSettings), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="settings"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateEstateSettingsAsync(
                System.Guid estateUid,
                RA.Customer.Types.Backend.Estate.EstateSettings settings,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateEstateSettingsAsync_estateUid_settings_cancellationToken, new object[] {estateUid, settings, cancellationToken}, () => impl.UpdateEstateSettingsAsync(estateUid, settings, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetImageContentAsync_estateUid_imageUid_cancellationToken = GetMethodInfo("GetImageContentAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="imageUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetImageContentAsync(
                System.Guid estateUid,
                System.Guid imageUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetImageContentAsync_estateUid_imageUid_cancellationToken, new object[] {estateUid, imageUid, cancellationToken}, () => impl.GetImageContentAsync(estateUid, imageUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertImageContentAsync_estateUid_data_cancellationToken = GetMethodInfo("UpsertImageContentAsync", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.FileContent), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="data"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertImageContentAsync(
                System.Guid estateUid,
                RA.Customer.Types.FileContent data,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertImageContentAsync_estateUid_data_cancellationToken, new object[] {estateUid, data, cancellationToken}, () => impl.UpsertImageContentAsync(estateUid, data, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetDocumentAsync_estateUid_documentUid_cancellationToken = GetMethodInfo("GetDocumentAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="documentUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetDocumentAsync(
                System.Guid estateUid,
                System.Guid documentUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetDocumentAsync_estateUid_documentUid_cancellationToken, new object[] {estateUid, documentUid, cancellationToken}, () => impl.GetDocumentAsync(estateUid, documentUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertDocumentAsync_estateUid_data_cancellationToken = GetMethodInfo("UpsertDocumentAsync", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.FileContent), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="data"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertDocumentAsync(
                System.Guid estateUid,
                RA.Customer.Types.FileContent data,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertDocumentAsync_estateUid_data_cancellationToken, new object[] {estateUid, data, cancellationToken}, () => impl.UpsertDocumentAsync(estateUid, data, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetDocumentTypes_cancellationToken = GetMethodInfo("GetDocumentTypes", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [System.Obsolete]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateDocumentType>> GetDocumentTypes(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateDocumentType>>(_serviceProvider, impl, mi_GetDocumentTypes_cancellationToken, new object[] {cancellationToken}, () => impl.GetDocumentTypes(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetDocumentTypesAsync_cancellationToken = GetMethodInfo("GetDocumentTypesAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateDocumentType>> GetDocumentTypesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateDocumentType>>(_serviceProvider, impl, mi_GetDocumentTypesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetDocumentTypesAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SetDocumentTypeAsync_estateUid_documentUid_documentType_cancellationToken = GetMethodInfo("SetDocumentTypeAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="documentUid"></param>
            /// <param name="documentType"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetDocumentTypeAsync(
                System.Guid estateUid,
                System.Guid documentUid,
                System.Guid documentType,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetDocumentTypeAsync_estateUid_documentUid_documentType_cancellationToken, new object[] {estateUid, documentUid, documentType, cancellationToken}, () => impl.SetDocumentTypeAsync(estateUid, documentUid, documentType, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_MakeDocumentAvailableInStepsAsync_estateUid_documentUid_view_steps_cancellationToken = GetMethodInfo("MakeDocumentAvailableInStepsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.String), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="documentUid"></param>
            /// <param name="view"></param>
            /// <param name="steps"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task MakeDocumentAvailableInStepsAsync(
                System.Guid estateUid,
                System.Guid documentUid,
                System.String view,
                System.Collections.Generic.IEnumerable<System.String> steps,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_MakeDocumentAvailableInStepsAsync_estateUid_documentUid_view_steps_cancellationToken, new object[] {estateUid, documentUid, view, steps, cancellationToken}, () => impl.MakeDocumentAvailableInStepsAsync(estateUid, documentUid, view, steps, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertViewingsAsync_estateId_estateViewings_cancellationToken = GetMethodInfo("UpsertViewingsAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateViewing>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="estateViewings"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertViewingsAsync(
                RA.Customer.Types.Backend.ExternalID estateId,
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateViewing> estateViewings,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertViewingsAsync_estateId_estateViewings_cancellationToken, new object[] {estateId, estateViewings, cancellationToken}, () => impl.UpsertViewingsAsync(estateId, estateViewings, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertViewingParticipantsAsync_estateId_viewingId_participants_cancellationToken = GetMethodInfo("UpsertViewingParticipantsAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateViewingParticipant>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="viewingId"></param>
            /// <param name="participants"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertViewingParticipantsAsync(
                RA.Customer.Types.Backend.ExternalID estateId,
                RA.Customer.Types.Backend.ExternalID viewingId,
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateViewingParticipant> participants,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertViewingParticipantsAsync_estateId_viewingId_participants_cancellationToken, new object[] {estateId, viewingId, participants, cancellationToken}, () => impl.UpsertViewingParticipantsAsync(estateId, viewingId, participants, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertBidsAsync_estateId_estateBids_cancellationToken = GetMethodInfo("UpsertBidsAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateBid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="estateBids"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertBidsAsync(
                RA.Customer.Types.Backend.ExternalID estateId,
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Estate.EstateBid> estateBids,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertBidsAsync_estateId_estateBids_cancellationToken, new object[] {estateId, estateBids, cancellationToken}, () => impl.UpsertBidsAsync(estateId, estateBids, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SyncWithBrokerSystemAsync_estateUid_cancellationToken = GetMethodInfo("SyncWithBrokerSystemAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> SyncWithBrokerSystemAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_SyncWithBrokerSystemAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.SyncWithBrokerSystemAsync(estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SyncWithBrokerSystemDetachedAsync_estateUid_cancellationToken = GetMethodInfo("SyncWithBrokerSystemDetachedAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SyncWithBrokerSystemDetachedAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SyncWithBrokerSystemDetachedAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.SyncWithBrokerSystemDetachedAsync(estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DownloadEPaperIfMissingAsync_estateUid_cancellationToken = GetMethodInfo("DownloadEPaperIfMissingAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DownloadEPaperIfMissingAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DownloadEPaperIfMissingAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.DownloadEPaperIfMissingAsync(estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_PlaceBid_estateUid_contactUid_amount_cancellationToken = GetMethodInfo("PlaceBid", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Decimal), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="contactUid"></param>
            /// <param name="amount"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Boolean> PlaceBid(
                System.Guid estateUid,
                System.Guid contactUid,
                System.Decimal amount,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Boolean>(_serviceProvider, impl, mi_PlaceBid_estateUid_contactUid_amount_cancellationToken, new object[] {estateUid, contactUid, amount, cancellationToken}, () => impl.PlaceBid(estateUid, contactUid, amount, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateLoanPromise_estateUid_contactUid_promise_cancellationToken = GetMethodInfo("UpdateLoanPromise", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(RA.Customer.Types.Backend.Estate.EstateInterestLoanPromise), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="contactUid"></param>
            /// <param name="promise"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateLoanPromise(
                System.Guid estateUid,
                System.Guid contactUid,
                RA.Customer.Types.Backend.Estate.EstateInterestLoanPromise promise,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateLoanPromise_estateUid_contactUid_promise_cancellationToken, new object[] {estateUid, contactUid, promise, cancellationToken}, () => impl.UpdateLoanPromise(estateUid, contactUid, promise, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateNewEstateAsync_externalId_cancellationToken = GetMethodInfo("CreateNewEstateAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> CreateNewEstateAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_CreateNewEstateAsync_externalId_cancellationToken, new object[] {externalId, cancellationToken}, () => impl.CreateNewEstateAsync(externalId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateUidsByExternalIdsAsync_externalIDs_fetchMissingEstate_cancellationToken = GetMethodInfo("GetEstateUidsByExternalIdsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.ExternalID>), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalIDs"></param>
            /// <param name="fetchMissingEstate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.Guid>> GetEstateUidsByExternalIdsAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.ExternalID> externalIDs,
                System.Boolean fetchMissingEstate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<System.Guid>>(_serviceProvider, impl, mi_GetEstateUidsByExternalIdsAsync_externalIDs_fetchMissingEstate_cancellationToken, new object[] {externalIDs, fetchMissingEstate, cancellationToken}, () => impl.GetEstateUidsByExternalIdsAsync(externalIDs, fetchMissingEstate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateEstateContactRelationAsync_estateUid_contactUid_contactChangeDate_cancellationToken = GetMethodInfo("UpdateEstateContactRelationAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.DateTimeOffset), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="contactUid"></param>
            /// <param name="contactChangeDate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateEstateContactRelationAsync(
                System.Guid estateUid,
                System.Guid contactUid,
                System.DateTimeOffset contactChangeDate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateEstateContactRelationAsync_estateUid_contactUid_contactChangeDate_cancellationToken, new object[] {estateUid, contactUid, contactChangeDate, cancellationToken}, () => impl.UpdateEstateContactRelationAsync(estateUid, contactUid, contactChangeDate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateMergedContactUidsAsync_estateUid_cancellationToken = GetMethodInfo("UpdateMergedContactUidsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> UpdateMergedContactUidsAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_UpdateMergedContactUidsAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.UpdateMergedContactUidsAsync(estateUid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIEstateStatisticsProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IEstateStatistics>,RA.Customer.Services.Backend.IEstateStatistics {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIEstateStatisticsProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IEstateStatistics> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetStatisticSourcesAsync_cancellationToken = GetMethodInfo("GetStatisticSourcesAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSource>> GetStatisticSourcesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSource>>(_serviceProvider, impl, mi_GetStatisticSourcesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetStatisticSourcesAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_MergeStatisticSourcesAsync_cancellationToken = GetMethodInfo("MergeStatisticSourcesAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task MergeStatisticSourcesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_MergeStatisticSourcesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.MergeStatisticSourcesAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_MergeStatisticSourcesAsync_keepSourceId_deleteSourceId_continuationToken_cancellationToken = GetMethodInfo("MergeStatisticSourcesAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="keepSourceId"></param>
            /// <param name="deleteSourceId"></param>
            /// <param name="continuationToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task MergeStatisticSourcesAsync(
                System.Guid keepSourceId,
                System.Guid deleteSourceId,
                System.String continuationToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_MergeStatisticSourcesAsync_keepSourceId_deleteSourceId_continuationToken_cancellationToken, new object[] {keepSourceId, deleteSourceId, continuationToken, cancellationToken}, () => impl.MergeStatisticSourcesAsync(keepSourceId, deleteSourceId, continuationToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertStatisticsSourceAsync_statisticsSource_cancellationToken = GetMethodInfo("UpsertStatisticsSourceAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.EstateStatistics.StatisticsSource), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="statisticsSource"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSource>> UpsertStatisticsSourceAsync(
                RA.Customer.Types.Backend.EstateStatistics.StatisticsSource statisticsSource,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSource>>(_serviceProvider, impl, mi_UpsertStatisticsSourceAsync_statisticsSource_cancellationToken, new object[] {statisticsSource, cancellationToken}, () => impl.UpsertStatisticsSourceAsync(statisticsSource, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DeleteStatisticsSourceAsync_uid_cancellationToken = GetMethodInfo("DeleteStatisticsSourceAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSource>> DeleteStatisticsSourceAsync(
                System.Guid uid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSource>>(_serviceProvider, impl, mi_DeleteStatisticsSourceAsync_uid_cancellationToken, new object[] {uid, cancellationToken}, () => impl.DeleteStatisticsSourceAsync(uid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertSourceDataAsync_stats_cancellationToken = GetMethodInfo("UpsertSourceDataAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSourceData>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="stats"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertSourceDataAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.StatisticsSourceData> stats,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertSourceDataAsync_stats_cancellationToken, new object[] {stats, cancellationToken}, () => impl.UpsertSourceDataAsync(stats, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertStoredSourceDataAsync_statsUid_startIdx_cancellationToken = GetMethodInfo("UpsertStoredSourceDataAsync", new System.Type[] {typeof(System.Guid), typeof(System.Int32), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="statsUid"></param>
            /// <param name="startIdx"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertStoredSourceDataAsync(
                System.Guid statsUid,
                System.Int32 startIdx,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertStoredSourceDataAsync_statsUid_startIdx_cancellationToken, new object[] {statsUid, startIdx, cancellationToken}, () => impl.UpsertStoredSourceDataAsync(statsUid, startIdx, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateStatisticsAsync_estateUid_startDate_endDate_cancellationToken = GetMethodInfo("GetEstateStatisticsAsync", new System.Type[] {typeof(System.Guid), typeof(System.DateTimeOffset), typeof(System.DateTimeOffset), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="startDate"></param>
            /// <param name="endDate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.EstateStatistics.EstateStatistics> GetEstateStatisticsAsync(
                System.Guid estateUid,
                System.DateTimeOffset startDate,
                System.DateTimeOffset endDate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.EstateStatistics.EstateStatistics>(_serviceProvider, impl, mi_GetEstateStatisticsAsync_estateUid_startDate_endDate_cancellationToken, new object[] {estateUid, startDate, endDate, cancellationToken}, () => impl.GetEstateStatisticsAsync(estateUid, startDate, endDate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DeleteEstateStatisticsAsync_estateUid_cancellationToken = GetMethodInfo("DeleteEstateStatisticsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteEstateStatisticsAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DeleteEstateStatisticsAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.DeleteEstateStatisticsAsync(estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertEstateStatisticsSourceAsync_estateUid_source_cancellationToken = GetMethodInfo("UpsertEstateStatisticsSourceAsync", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.Backend.EstateStatistics.EstateStatisticsSource), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="source"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertEstateStatisticsSourceAsync(
                System.Guid estateUid,
                RA.Customer.Types.Backend.EstateStatistics.EstateStatisticsSource source,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertEstateStatisticsSourceAsync_estateUid_source_cancellationToken, new object[] {estateUid, source, cancellationToken}, () => impl.UpsertEstateStatisticsSourceAsync(estateUid, source, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateStatisticsSourcesAsync_estateUid_cancellationToken = GetMethodInfo("GetEstateStatisticsSourcesAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.EstateStatisticsSource>> GetEstateStatisticsSourcesAsync(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatistics)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.EstateStatistics.EstateStatisticsSource>>(_serviceProvider, impl, mi_GetEstateStatisticsSourcesAsync_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.GetEstateStatisticsSourcesAsync(estateUid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIEstateStatisticsGoogleProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IEstateStatisticsGoogle>,RA.Customer.Services.Backend.IEstateStatisticsGoogle {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIEstateStatisticsGoogleProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IEstateStatisticsGoogle> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetStatisticsFromGoogleAsync_cancellationToken = GetMethodInfo("GetStatisticsFromGoogleAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task GetStatisticsFromGoogleAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatisticsGoogle)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_GetStatisticsFromGoogleAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetStatisticsFromGoogleAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetStatisticsFromGoogleAsync_day_cancellationToken = GetMethodInfo("GetStatisticsFromGoogleAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="day"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task GetStatisticsFromGoogleAsync(
                System.String day,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatisticsGoogle)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_GetStatisticsFromGoogleAsync_day_cancellationToken, new object[] {day, cancellationToken}, () => impl.GetStatisticsFromGoogleAsync(day, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetStatisticsForEstateFromGoogleAsync_estateId_startDay_endDay_cancellationToken = GetMethodInfo("GetStatisticsForEstateFromGoogleAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="startDay"></param>
            /// <param name="endDay"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task GetStatisticsForEstateFromGoogleAsync(
                System.String estateId,
                System.String startDay,
                System.String endDay,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatisticsGoogle)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_GetStatisticsForEstateFromGoogleAsync_estateId_startDay_endDay_cancellationToken, new object[] {estateId, startDay, endDay, cancellationToken}, () => impl.GetStatisticsForEstateFromGoogleAsync(estateId, startDay, endDay, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetStatisticsForEstateFromGoogleAsync_estateId_day_cancellationToken = GetMethodInfo("GetStatisticsForEstateFromGoogleAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="day"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task GetStatisticsForEstateFromGoogleAsync(
                System.String estateId,
                System.String day,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IEstateStatisticsGoogle)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_GetStatisticsForEstateFromGoogleAsync_estateId_day_cancellationToken, new object[] {estateId, day, cancellationToken}, () => impl.GetStatisticsForEstateFromGoogleAsync(estateId, day, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIMspecsProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IMspecs>,RA.Customer.Services.Backend.IMspecs {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIMspecsProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IMspecs> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_StructureUpdatedAsync_dataType_id_cancellationToken = GetMethodInfo("StructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
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
                var impl = (RA.Customer.Services.Backend.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_StructureUpdatedAsync_dataType_id_cancellationToken, new object[] {dataType, id, cancellationToken}, () => impl.StructureUpdatedAsync(dataType, id, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RegisterUnhandledDataTypeAsyc_dataType_cancellationToken = GetMethodInfo("RegisterUnhandledDataTypeAsyc", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dataType"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task RegisterUnhandledDataTypeAsyc(
                System.String dataType,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_RegisterUnhandledDataTypeAsyc_dataType_cancellationToken, new object[] {dataType, cancellationToken}, () => impl.RegisterUnhandledDataTypeAsyc(dataType, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ClearUnhandledDataTypesAsyc_cancellationToken = GetMethodInfo("ClearUnhandledDataTypesAsyc", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ClearUnhandledDataTypesAsyc(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ClearUnhandledDataTypesAsyc_cancellationToken, new object[] {cancellationToken}, () => impl.ClearUnhandledDataTypesAsyc(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetUnhandledDataTypesAsyc_cancellationToken = GetMethodInfo("GetUnhandledDataTypesAsyc", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.UnhandledDataType>> GetUnhandledDataTypesAsyc(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.UnhandledDataType>>(_serviceProvider, impl, mi_GetUnhandledDataTypesAsyc_cancellationToken, new object[] {cancellationToken}, () => impl.GetUnhandledDataTypesAsyc(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingUriAsync_dealId_contactId_cancellationToken = GetMethodInfo("GetBiddingUriAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dealId"></param>
            /// <param name="contactId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> GetBiddingUriAsync(
                System.String dealId,
                System.String contactId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IMspecs)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetBiddingUriAsync_dealId_contactId_cancellationToken, new object[] {dealId, contactId, cancellationToken}, () => impl.GetBiddingUriAsync(dealId, contactId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendINotificationProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.INotification>,RA.Customer.Services.Backend.INotification {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendINotificationProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.INotification> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetCommonNotificationTemplatesAsync_cancellationToken = GetMethodInfo("GetCommonNotificationTemplatesAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.NotificationTemplate>> GetCommonNotificationTemplatesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.NotificationTemplate>>(_serviceProvider, impl, mi_GetCommonNotificationTemplatesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetCommonNotificationTemplatesAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertCommonNotificationTemplatesAsync_templates_cancellationToken = GetMethodInfo("UpsertCommonNotificationTemplatesAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.NotificationTemplate>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="templates"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertCommonNotificationTemplatesAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.NotificationTemplate> templates,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertCommonNotificationTemplatesAsync_templates_cancellationToken, new object[] {templates, cancellationToken}, () => impl.UpsertCommonNotificationTemplatesAsync(templates, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactNotificationsAsync_contactUid_cancellationToken = GetMethodInfo("GetContactNotificationsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.Notification>> GetContactNotificationsAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.Notification>>(_serviceProvider, impl, mi_GetContactNotificationsAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.GetContactNotificationsAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetContactNotificationChangeTokenAsync_contactUid_cancellationToken = GetMethodInfo("GetContactNotificationChangeTokenAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> GetContactNotificationChangeTokenAsync(
                System.Guid contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_GetContactNotificationChangeTokenAsync_contactUid_cancellationToken, new object[] {contactUid, cancellationToken}, () => impl.GetContactNotificationChangeTokenAsync(contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertContactNotificationsAsync_notifications_contactUid_cancellationToken = GetMethodInfo("UpsertContactNotificationsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.Notification>), typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="notifications"></param>
            /// <param name="contactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertContactNotificationsAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.Notification> notifications,
                System.Guid? contactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertContactNotificationsAsync_notifications_contactUid_cancellationToken, new object[] {notifications, contactUid, cancellationToken}, () => impl.UpsertContactNotificationsAsync(notifications, contactUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AddContactNotificationAsync_notification_cancellationToken = GetMethodInfo("AddContactNotificationAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Notification.Notification), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task AddContactNotificationAsync(
                RA.Customer.Types.Backend.Notification.Notification notification,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_AddContactNotificationAsync_notification_cancellationToken, new object[] {notification, cancellationToken}, () => impl.AddContactNotificationAsync(notification, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AddContactNotificationsAsync_notifications_cancellationToken = GetMethodInfo("AddContactNotificationsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.Notification>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="notifications"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task AddContactNotificationsAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Notification.Notification> notifications,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_AddContactNotificationsAsync_notifications_cancellationToken, new object[] {notifications, cancellationToken}, () => impl.AddContactNotificationsAsync(notifications, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_MarkContactNotificationsAsReadAsync_contactId_notificationIds_cancellationToken = GetMethodInfo("MarkContactNotificationsAsReadAsync", new System.Type[] {typeof(System.Guid), typeof(System.Collections.Generic.IEnumerable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactId"></param>
            /// <param name="notificationIds"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task MarkContactNotificationsAsReadAsync(
                System.Guid contactId,
                System.Collections.Generic.IEnumerable<System.Guid> notificationIds,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_MarkContactNotificationsAsReadAsync_contactId_notificationIds_cancellationToken, new object[] {contactId, notificationIds, cancellationToken}, () => impl.MarkContactNotificationsAsReadAsync(contactId, notificationIds, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateNotificationAsync_contactUid_title_text_sender_link_cancellationToken = GetMethodInfo("CreateNotificationAsync", new System.Type[] {typeof(System.Guid), typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Uri), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="title"></param>
            /// <param name="text"></param>
            /// <param name="sender"></param>
            /// <param name="link"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Notification.Notification> CreateNotificationAsync(
                System.Guid contactUid,
                System.String title,
                System.String text,
                System.String sender,
                System.Uri link,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Notification.Notification>(_serviceProvider, impl, mi_CreateNotificationAsync_contactUid_title_text_sender_link_cancellationToken, new object[] {contactUid, title, text, sender, link, cancellationToken}, () => impl.CreateNotificationAsync(contactUid, title, text, sender, link, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateTemplateNotificationDeferredAsync_contactUid_templateUid_estateUid_documentUid_cancellationToken = GetMethodInfo("CreateTemplateNotificationDeferredAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Nullable<System.Guid>), typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="templateUid"></param>
            /// <param name="estateUid"></param>
            /// <param name="documentUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CreateTemplateNotificationDeferredAsync(
                System.Guid contactUid,
                System.Guid templateUid,
                System.Guid? estateUid,
                System.Guid? documentUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CreateTemplateNotificationDeferredAsync_contactUid_templateUid_estateUid_documentUid_cancellationToken, new object[] {contactUid, templateUid, estateUid, documentUid, cancellationToken}, () => impl.CreateTemplateNotificationDeferredAsync(contactUid, templateUid, estateUid, documentUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateTemplateNotificationAsync_contactUid_templateUid_estateUid_documentUid_cancellationToken = GetMethodInfo("CreateTemplateNotificationAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Nullable<System.Guid>), typeof(System.Nullable<System.Guid>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="templateUid"></param>
            /// <param name="estateUid"></param>
            /// <param name="documentUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Notification.Notification> CreateTemplateNotificationAsync(
                System.Guid contactUid,
                System.Guid templateUid,
                System.Guid? estateUid,
                System.Guid? documentUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Notification.Notification>(_serviceProvider, impl, mi_CreateTemplateNotificationAsync_contactUid_templateUid_estateUid_documentUid_cancellationToken, new object[] {contactUid, templateUid, estateUid, documentUid, cancellationToken}, () => impl.CreateTemplateNotificationAsync(contactUid, templateUid, estateUid, documentUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CopyNotificationsAsync_fromContactUid_toContactUid_cancellationToken = GetMethodInfo("CopyNotificationsAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fromContactUid"></param>
            /// <param name="toContactUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CopyNotificationsAsync(
                System.Guid fromContactUid,
                System.Guid toContactUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.INotification)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CopyNotificationsAsync_fromContactUid_toContactUid_cancellationToken, new object[] {fromContactUid, toContactUid, cancellationToken}, () => impl.CopyNotificationsAsync(fromContactUid, toContactUid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIOfficeProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IOffice>,RA.Customer.Services.Backend.IOffice {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIOfficeProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IOffice> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetOfficeByUidAsync_officeUid_mustExist_cancellationToken = GetMethodInfo("GetOfficeByUidAsync", new System.Type[] {typeof(System.Guid), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="officeUid"></param>
            /// <param name="mustExist"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Office.Office> GetOfficeByUidAsync(
                System.Guid officeUid,
                System.Boolean mustExist,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Office.Office>(_serviceProvider, impl, mi_GetOfficeByUidAsync_officeUid_mustExist_cancellationToken, new object[] {officeUid, mustExist, cancellationToken}, () => impl.GetOfficeByUidAsync(officeUid, mustExist, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetOfficesAsync_cancellationToken = GetMethodInfo("GetOfficesAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Office.Office>> GetOfficesAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Office.Office>>(_serviceProvider, impl, mi_GetOfficesAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetOfficesAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetOfficeByExternalIdAsync_externalId_mustExist_fetchMissingOffice_cancellationToken = GetMethodInfo("GetOfficeByExternalIdAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="mustExist"></param>
            /// <param name="fetchMissingOffice"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Office.Office> GetOfficeByExternalIdAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Boolean mustExist,
                System.Boolean fetchMissingOffice,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Office.Office>(_serviceProvider, impl, mi_GetOfficeByExternalIdAsync_externalId_mustExist_fetchMissingOffice_cancellationToken, new object[] {externalId, mustExist, fetchMissingOffice, cancellationToken}, () => impl.GetOfficeByExternalIdAsync(externalId, mustExist, fetchMissingOffice, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertOfficeAsync_office_cancellationToken = GetMethodInfo("UpsertOfficeAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Office.Office), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="office"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Office.Office> UpsertOfficeAsync(
                RA.Customer.Types.Backend.Office.Office office,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Office.Office>(_serviceProvider, impl, mi_UpsertOfficeAsync_office_cancellationToken, new object[] {office, cancellationToken}, () => impl.UpsertOfficeAsync(office, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckOfficesAsync_continuationToken_cancellationToken = GetMethodInfo("CheckOfficesAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="continuationToken"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckOfficesAsync(
                System.String continuationToken,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckOfficesAsync_continuationToken_cancellationToken, new object[] {continuationToken, cancellationToken}, () => impl.CheckOfficesAsync(continuationToken, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CheckOfficeAsync_officeUid_cancellationToken = GetMethodInfo("CheckOfficeAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="officeUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CheckOfficeAsync(
                System.Guid officeUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CheckOfficeAsync_officeUid_cancellationToken, new object[] {officeUid, cancellationToken}, () => impl.CheckOfficeAsync(officeUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateNewOfficeAsync_externalId_cancellationToken = GetMethodInfo("CreateNewOfficeAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.ExternalID), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="externalId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Office.Office> CreateNewOfficeAsync(
                RA.Customer.Types.Backend.ExternalID externalId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IOffice)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Office.Office>(_serviceProvider, impl, mi_CreateNewOfficeAsync_externalId_cancellationToken, new object[] {externalId, cancellationToken}, () => impl.CreateNewOfficeAsync(externalId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIResourcesProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IResources>,RA.Customer.Services.Backend.IResources {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIResourcesProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IResources> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateImageUriAsync_estateUid_imageUid_transformOps_cancellationToken = GetMethodInfo("GetEstateImageUriAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="imageUid"></param>
            /// <param name="transformOps"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> GetEstateImageUriAsync(
                System.Guid estateUid,
                System.Guid imageUid,
                System.Collections.Generic.IEnumerable<System.String> transformOps,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IResources)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetEstateImageUriAsync_estateUid_imageUid_transformOps_cancellationToken, new object[] {estateUid, imageUid, transformOps, cancellationToken}, () => impl.GetEstateImageUriAsync(estateUid, imageUid, transformOps, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateDocumentUriAsync_estateUid_documentUid_fileName_cancellationToken = GetMethodInfo("GetEstateDocumentUriAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="documentUid"></param>
            /// <param name="fileName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> GetEstateDocumentUriAsync(
                System.Guid estateUid,
                System.Guid documentUid,
                System.String fileName,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IResources)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetEstateDocumentUriAsync_estateUid_documentUid_fileName_cancellationToken, new object[] {estateUid, documentUid, fileName, cancellationToken}, () => impl.GetEstateDocumentUriAsync(estateUid, documentUid, fileName, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBrokerImageUriAsync_brokerUid_transformOps_cancellationToken = GetMethodInfo("GetBrokerImageUriAsync", new System.Type[] {typeof(System.Guid), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="brokerUid"></param>
            /// <param name="transformOps"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> GetBrokerImageUriAsync(
                System.Guid brokerUid,
                System.Collections.Generic.IEnumerable<System.String> transformOps,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IResources)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetBrokerImageUriAsync_brokerUid_transformOps_cancellationToken, new object[] {brokerUid, transformOps, cancellationToken}, () => impl.GetBrokerImageUriAsync(brokerUid, transformOps, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendISettingsProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.ISettings>,RA.Customer.Services.Backend.ISettings {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendISettingsProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.ISettings> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSettingsAsync_cancellationToken = GetMethodInfo("GetSettingsAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Settings.Settings> GetSettingsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ISettings)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Settings.Settings>(_serviceProvider, impl, mi_GetSettingsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetSettingsAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertSettingsAsync_settings_cancellationToken = GetMethodInfo("UpsertSettingsAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Settings.Settings), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="settings"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Settings.Settings> UpsertSettingsAsync(
                RA.Customer.Types.Backend.Settings.Settings settings,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ISettings)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Settings.Settings>(_serviceProvider, impl, mi_UpsertSettingsAsync_settings_cancellationToken, new object[] {settings, cancellationToken}, () => impl.UpsertSettingsAsync(settings, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendISPBolanProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.ISPBolan>,RA.Customer.Services.Backend.ISPBolan {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendISPBolanProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.ISPBolan> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SendContactEstateLeadAsync_contactUid_estateUid_cancellationToken = GetMethodInfo("SendContactEstateLeadAsync", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactUid"></param>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SendContactEstateLeadAsync(
                System.Guid contactUid,
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ISPBolan)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SendContactEstateLeadAsync_contactUid_estateUid_cancellationToken, new object[] {contactUid, estateUid, cancellationToken}, () => impl.SendContactEstateLeadAsync(contactUid, estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SendBolanLeadAsync_leadUid_cancellationToken = GetMethodInfo("SendBolanLeadAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="leadUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SendBolanLeadAsync(
                System.Guid leadUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ISPBolan)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SendBolanLeadAsync_leadUid_cancellationToken, new object[] {leadUid, cancellationToken}, () => impl.SendBolanLeadAsync(leadUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBolanLeadAsync_leadUid_cancellationToken = GetMethodInfo("GetBolanLeadAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="leadUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Bolan.BolanLead> GetBolanLeadAsync(
                System.Guid leadUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ISPBolan)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Bolan.BolanLead>(_serviceProvider, impl, mi_GetBolanLeadAsync_leadUid_cancellationToken, new object[] {leadUid, cancellationToken}, () => impl.GetBolanLeadAsync(leadUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBolanLeadsAsync_createdAfterDate_cancellationToken = GetMethodInfo("GetBolanLeadsAsync", new System.Type[] {typeof(System.Nullable<System.DateTimeOffset>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="createdAfterDate"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Bolan.BolanLead>> GetBolanLeadsAsync(
                System.DateTimeOffset? createdAfterDate,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ISPBolan)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Bolan.BolanLead>>(_serviceProvider, impl, mi_GetBolanLeadsAsync_createdAfterDate_cancellationToken, new object[] {createdAfterDate, cancellationToken}, () => impl.GetBolanLeadsAsync(createdAfterDate, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertBolanLeadAsync_bolanLead_cancellationToken = GetMethodInfo("UpsertBolanLeadAsync", new System.Type[] {typeof(RA.Customer.Types.Backend.Bolan.BolanLead), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="bolanLead"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Bolan.BolanLead> UpsertBolanLeadAsync(
                RA.Customer.Types.Backend.Bolan.BolanLead bolanLead,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ISPBolan)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Bolan.BolanLead>(_serviceProvider, impl, mi_UpsertBolanLeadAsync_bolanLead_cancellationToken, new object[] {bolanLead, cancellationToken}, () => impl.UpsertBolanLeadAsync(bolanLead, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIStepProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IStep>,RA.Customer.Services.Backend.IStep {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIStepProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IStep> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ListStepsAsync_cancellationToken = GetMethodInfo("ListStepsAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Step.Step>> ListStepsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IStep)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Step.Step>>(_serviceProvider, impl, mi_ListStepsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.ListStepsAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetStepAsync_viewName_stepName_cancellationToken = GetMethodInfo("GetStepAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="viewName"></param>
            /// <param name="stepName"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Step.Step> GetStepAsync(
                System.String viewName,
                System.String stepName,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IStep)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Step.Step>(_serviceProvider, impl, mi_GetStepAsync_viewName_stepName_cancellationToken, new object[] {viewName, stepName, cancellationToken}, () => impl.GetStepAsync(viewName, stepName, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertStepsAsync_steps_cancellationToken = GetMethodInfo("UpsertStepsAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Step.Step>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="steps"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertStepsAsync(
                System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.Step.Step> steps,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IStep)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertStepsAsync_steps_cancellationToken, new object[] {steps, cancellationToken}, () => impl.UpsertStepsAsync(steps, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIThemeProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.ITheme>,RA.Customer.Services.Backend.ITheme {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIThemeProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.ITheme> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ResolveEnvAsync_cancellation = GetMethodInfo("ResolveEnvAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.String>> ResolveEnvAsync(
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Backend.ITheme)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<System.String>>(_serviceProvider, impl, mi_ResolveEnvAsync_cancellation, new object[] {cancellation}, () => impl.ResolveEnvAsync(cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetThemesAsync_cancellation = GetMethodInfo("GetThemesAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Theme>> GetThemesAsync(
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Backend.ITheme)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Theme>>(_serviceProvider, impl, mi_GetThemesAsync_cancellation, new object[] {cancellation}, () => impl.GetThemesAsync(cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ResolveThemeAsync_env_cancellation = GetMethodInfo("ResolveThemeAsync", new System.Type[] {typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="env"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Theme> ResolveThemeAsync(
                System.Collections.Generic.IEnumerable<System.String> env,
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Backend.ITheme)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Theme>(_serviceProvider, impl, mi_ResolveThemeAsync_env_cancellation, new object[] {env, cancellation}, () => impl.ResolveThemeAsync(env, cancellation));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertThemeAsync_theme_cancellation = GetMethodInfo("UpsertThemeAsync", new System.Type[] {typeof(RA.Customer.Types.Theme), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="theme"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Theme> UpsertThemeAsync(
                RA.Customer.Types.Theme theme,
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Backend.ITheme)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Theme>(_serviceProvider, impl, mi_UpsertThemeAsync_theme_cancellation, new object[] {theme, cancellation}, () => impl.UpsertThemeAsync(theme, cancellation));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendITinyUrlProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.ITinyUrl>,RA.Customer.Services.Backend.ITinyUrl {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendITinyUrlProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.ITinyUrl> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateTinyUrlAsync_uri_cancellationToken = GetMethodInfo("CreateTinyUrlAsync", new System.Type[] {typeof(System.Uri), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> CreateTinyUrlAsync(
                System.Uri uri,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ITinyUrl)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_CreateTinyUrlAsync_uri_cancellationToken, new object[] {uri, cancellationToken}, () => impl.CreateTinyUrlAsync(uri, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CreateTinyIdAsync_uri_cancellationToken = GetMethodInfo("CreateTinyIdAsync", new System.Type[] {typeof(System.Uri), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.TinyUrl.TinyUrl> CreateTinyIdAsync(
                System.Uri uri,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ITinyUrl)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.TinyUrl.TinyUrl>(_serviceProvider, impl, mi_CreateTinyIdAsync_uri_cancellationToken, new object[] {uri, cancellationToken}, () => impl.CreateTinyIdAsync(uri, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RedirectAsync_tinyId_cancellationToken = GetMethodInfo("RedirectAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="tinyId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> RedirectAsync(
                System.String tinyId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.ITinyUrl)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_RedirectAsync_tinyId_cancellationToken, new object[] {tinyId, cancellationToken}, () => impl.RedirectAsync(tinyId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIVitecProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IVitec>,RA.Customer.Services.Backend.IVitec {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIVitecProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IVitec> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetEstateUidAsync_vitecId_cancellationToken = GetMethodInfo("GetEstateUidAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Guid> GetEstateUidAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Guid>(_serviceProvider, impl, mi_GetEstateUidAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.GetEstateUidAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ActivateEstateMyPagesAsync_vitecId_cancellationToken = GetMethodInfo("ActivateEstateMyPagesAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> ActivateEstateMyPagesAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_ActivateEstateMyPagesAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.ActivateEstateMyPagesAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateEstateAsync_vitecId_mustExist_cancellationToken = GetMethodInfo("UpdateEstateAsync", new System.Type[] {typeof(System.String), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="mustExist"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Estate.Estate> UpdateEstateAsync(
                System.String vitecId,
                System.Boolean mustExist,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Estate.Estate>(_serviceProvider, impl, mi_UpdateEstateAsync_vitecId_mustExist_cancellationToken, new object[] {vitecId, mustExist, cancellationToken}, () => impl.UpdateEstateAsync(vitecId, mustExist, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateContactAsync_vitecId_mustExist_cancellationToken = GetMethodInfo("UpdateContactAsync", new System.Type[] {typeof(System.String), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="mustExist"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Contact.Contact> UpdateContactAsync(
                System.String vitecId,
                System.Boolean mustExist,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Contact.Contact>(_serviceProvider, impl, mi_UpdateContactAsync_vitecId_mustExist_cancellationToken, new object[] {vitecId, mustExist, cancellationToken}, () => impl.UpdateContactAsync(vitecId, mustExist, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateBrokerAsync_vitecId_mustExist_cancellationToken = GetMethodInfo("UpdateBrokerAsync", new System.Type[] {typeof(System.String), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="mustExist"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Broker.Broker> UpdateBrokerAsync(
                System.String vitecId,
                System.Boolean mustExist,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Broker.Broker>(_serviceProvider, impl, mi_UpdateBrokerAsync_vitecId_mustExist_cancellationToken, new object[] {vitecId, mustExist, cancellationToken}, () => impl.UpdateBrokerAsync(vitecId, mustExist, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateBrokersAsync_cancellationToken = GetMethodInfo("UpdateBrokersAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateBrokersAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateBrokersAsync_cancellationToken, new object[] {cancellationToken}, () => impl.UpdateBrokersAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateOfficeAsync_vitecId_mustExist_cancellationToken = GetMethodInfo("UpdateOfficeAsync", new System.Type[] {typeof(System.String), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="mustExist"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Backend.Office.Office> UpdateOfficeAsync(
                System.String vitecId,
                System.Boolean mustExist,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Backend.Office.Office>(_serviceProvider, impl, mi_UpdateOfficeAsync_vitecId_mustExist_cancellationToken, new object[] {vitecId, mustExist, cancellationToken}, () => impl.UpdateOfficeAsync(vitecId, mustExist, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetFileAsync_estateId_fileId_cancellationToken = GetMethodInfo("GetFileAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="fileId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetFileAsync(
                System.String estateId,
                System.String fileId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetFileAsync_estateId_fileId_cancellationToken, new object[] {estateId, fileId, cancellationToken}, () => impl.GetFileAsync(estateId, fileId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetImageAsync_estateId_imageId_cancellationToken = GetMethodInfo("GetImageAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="imageId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetImageAsync(
                System.String estateId,
                System.String imageId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetImageAsync_estateId_imageId_cancellationToken, new object[] {estateId, imageId, cancellationToken}, () => impl.GetImageAsync(estateId, imageId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBrokerImageAsync_id_cancellationToken = GetMethodInfo("GetBrokerImageAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.FileContent> GetBrokerImageAsync(
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.FileContent>(_serviceProvider, impl, mi_GetBrokerImageAsync_id_cancellationToken, new object[] {id, cancellationToken}, () => impl.GetBrokerImageAsync(id, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetWebtopEPaperLinkAsync_id_cancellationToken = GetMethodInfo("GetWebtopEPaperLinkAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Uri> GetWebtopEPaperLinkAsync(
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Uri>(_serviceProvider, impl, mi_GetWebtopEPaperLinkAsync_id_cancellationToken, new object[] {id, cancellationToken}, () => impl.GetWebtopEPaperLinkAsync(id, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_PlaceBidAsync_estateId_contactIds_amount_cancellationToken = GetMethodInfo("PlaceBidAsync", new System.Type[] {typeof(System.String), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Decimal), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="contactIds"></param>
            /// <param name="amount"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Boolean> PlaceBidAsync(
                System.String estateId,
                System.Collections.Generic.IEnumerable<System.String> contactIds,
                System.Decimal amount,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Boolean>(_serviceProvider, impl, mi_PlaceBidAsync_estateId_contactIds_amount_cancellationToken, new object[] {estateId, contactIds, amount, cancellationToken}, () => impl.PlaceBidAsync(estateId, contactIds, amount, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SetContactSsnAsync_contactId_ssn_cancellationToken = GetMethodInfo("SetContactSsnAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactId"></param>
            /// <param name="ssn"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetContactSsnAsync(
                System.String contactId,
                System.String ssn,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetContactSsnAsync_contactId_ssn_cancellationToken, new object[] {contactId, ssn, cancellationToken}, () => impl.SetContactSsnAsync(contactId, ssn, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateContactEstatesAsync_contactId_cancellationToken = GetMethodInfo("UpdateContactEstatesAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="contactId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateContactEstatesAsync(
                System.String contactId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateContactEstatesAsync_contactId_cancellationToken, new object[] {contactId, cancellationToken}, () => impl.UpdateContactEstatesAsync(contactId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpdateEstateContactsAsync_id_cancellationToken = GetMethodInfo("UpdateEstateContactsAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateEstateContactsAsync(
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitec)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateEstateContactsAsync_id_cancellationToken, new object[] {id, cancellationToken}, () => impl.UpdateEstateContactsAsync(id, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesBackendIVitecUpdateProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Backend.IVitecUpdate>,RA.Customer.Services.Backend.IVitecUpdate {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesBackendIVitecUpdateProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Backend.IVitecUpdate> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_StructureUpdatedAsync_dataType_id_cancellationToken = GetMethodInfo("StructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
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
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_StructureUpdatedAsync_dataType_id_cancellationToken, new object[] {dataType, id, cancellationToken}, () => impl.StructureUpdatedAsync(dataType, id, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_RegisterUnhandledDataTypeAsyc_dataType_cancellationToken = GetMethodInfo("RegisterUnhandledDataTypeAsyc", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dataType"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task RegisterUnhandledDataTypeAsyc(
                System.String dataType,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_RegisterUnhandledDataTypeAsyc_dataType_cancellationToken, new object[] {dataType, cancellationToken}, () => impl.RegisterUnhandledDataTypeAsyc(dataType, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ClearUnhandledDataTypesAsyc_cancellationToken = GetMethodInfo("ClearUnhandledDataTypesAsyc", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ClearUnhandledDataTypesAsyc(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ClearUnhandledDataTypesAsyc_cancellationToken, new object[] {cancellationToken}, () => impl.ClearUnhandledDataTypesAsyc(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetUnhandledDataTypesAsyc_cancellationToken = GetMethodInfo("GetUnhandledDataTypesAsyc", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.UnhandledDataType>> GetUnhandledDataTypesAsyc(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Backend.UnhandledDataType>>(_serviceProvider, impl, mi_GetUnhandledDataTypesAsyc_cancellationToken, new object[] {cancellationToken}, () => impl.GetUnhandledDataTypesAsyc(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SimulateEstateUpdateAsync_vitecId_cancellationToken = GetMethodInfo("SimulateEstateUpdateAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SimulateEstateUpdateAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SimulateEstateUpdateAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.SimulateEstateUpdateAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_EstateStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("EstateStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task EstateStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_EstateStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.EstateStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_DepositStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("DepositStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DepositStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DepositStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.DepositStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SalesReportStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("SalesReportStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SalesReportStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SalesReportStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.SalesReportStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ContactStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("ContactStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ContactStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ContactStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.ContactStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ViewingsStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("ViewingsStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ViewingsStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ViewingsStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.ViewingsStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_FilesStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("FilesStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task FilesStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_FilesStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.FilesStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UserStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("UserStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UserStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UserStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.UserStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_OfficeStructureUpdatedAsync_vitecId_cancellationToken = GetMethodInfo("OfficeStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vitecId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task OfficeStructureUpdatedAsync(
                System.String vitecId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_OfficeStructureUpdatedAsync_vitecId_cancellationToken, new object[] {vitecId, cancellationToken}, () => impl.OfficeStructureUpdatedAsync(vitecId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_BidStructureUpdatedAsync_estateId_bidId_cancellationToken = GetMethodInfo("BidStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="bidId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task BidStructureUpdatedAsync(
                System.String estateId,
                System.String bidId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_BidStructureUpdatedAsync_estateId_bidId_cancellationToken, new object[] {estateId, bidId, cancellationToken}, () => impl.BidStructureUpdatedAsync(estateId, bidId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_BiddingSettingStructureUpdatedAsync_estateId_cancellationToken = GetMethodInfo("BiddingSettingStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task BiddingSettingStructureUpdatedAsync(
                System.String estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_BiddingSettingStructureUpdatedAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.BiddingSettingStructureUpdatedAsync(estateId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ContractStructureUpdatedAsync_id_cancellationToken = GetMethodInfo("ContractStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ContractStructureUpdatedAsync(
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ContractStructureUpdatedAsync_id_cancellationToken, new object[] {id, cancellationToken}, () => impl.ContractStructureUpdatedAsync(id, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AccessStructureUpdatedAsync_id_cancellationToken = GetMethodInfo("AccessStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task AccessStructureUpdatedAsync(
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_AccessStructureUpdatedAsync_id_cancellationToken, new object[] {id, cancellationToken}, () => impl.AccessStructureUpdatedAsync(id, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CommissionAgentsStructureUpdatedAsync_id_cancellationToken = GetMethodInfo("CommissionAgentsStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CommissionAgentsStructureUpdatedAsync(
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CommissionAgentsStructureUpdatedAsync_id_cancellationToken, new object[] {id, cancellationToken}, () => impl.CommissionAgentsStructureUpdatedAsync(id, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CommissionAdjustmentsStructureUpdatedAsync_id_cancellationToken = GetMethodInfo("CommissionAdjustmentsStructureUpdatedAsync", new System.Type[] {typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CommissionAdjustmentsStructureUpdatedAsync(
                System.String id,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Backend.IVitecUpdate)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CommissionAdjustmentsStructureUpdatedAsync_id_cancellationToken, new object[] {id, cancellationToken}, () => impl.CommissionAdjustmentsStructureUpdatedAsync(id, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIChecklistViewProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IChecklistView>,RA.Customer.Services.Authenticated.IChecklistView {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIChecklistViewProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IChecklistView> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetChecklistsForEstate_estateId_cancellationToken = GetMethodInfo("GetChecklistsForEstate", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Checklist.ChecklistStep>> GetChecklistsForEstate(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IChecklistView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Checklist.ChecklistStep>>(_serviceProvider, impl, mi_GetChecklistsForEstate_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetChecklistsForEstate(estateId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetChecklist_estateId_checklistId_cancellationToken = GetMethodInfo("GetChecklist", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="checklistId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Checklist.Checklist> GetChecklist(
                System.Guid estateId,
                System.Guid checklistId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IChecklistView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Checklist.Checklist>(_serviceProvider, impl, mi_GetChecklist_estateId_checklistId_cancellationToken, new object[] {estateId, checklistId, cancellationToken}, () => impl.GetChecklist(estateId, checklistId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SetChecklistItemState_estateId_checklistId_checklistItemId_checkedState_cancellationToken = GetMethodInfo("SetChecklistItemState", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(System.Guid), typeof(System.Boolean), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="checklistId"></param>
            /// <param name="checklistItemId"></param>
            /// <param name="checkedState"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SetChecklistItemState(
                System.Guid estateId,
                System.Guid checklistId,
                System.Guid checklistItemId,
                System.Boolean checkedState,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IChecklistView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SetChecklistItemState_estateId_checklistId_checklistItemId_checkedState_cancellationToken, new object[] {estateId, checklistId, checklistItemId, checkedState, cancellationToken}, () => impl.SetChecklistItemState(estateId, checklistId, checklistItemId, checkedState, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AddChecklistItem_estateId_checklistId_item_cancellationToken = GetMethodInfo("AddChecklistItem", new System.Type[] {typeof(System.Guid), typeof(System.Guid), typeof(RA.Customer.Types.Checklist.ChecklistItem), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="checklistId"></param>
            /// <param name="item"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task AddChecklistItem(
                System.Guid estateId,
                System.Guid checklistId,
                RA.Customer.Types.Checklist.ChecklistItem item,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IChecklistView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_AddChecklistItem_estateId_checklistId_item_cancellationToken, new object[] {estateId, checklistId, item, cancellationToken}, () => impl.AddChecklistItem(estateId, checklistId, item, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIDocumentViewProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IDocumentView>,RA.Customer.Services.Authenticated.IDocumentView {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIDocumentViewProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IDocumentView> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetDocumentsForEstate_estateId_cancellationToken = GetMethodInfo("GetDocumentsForEstate", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.Document.Document>> GetDocumentsForEstate(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IDocumentView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.Document.Document>>(_serviceProvider, impl, mi_GetDocumentsForEstate_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetDocumentsForEstate(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIBiddingViewProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IBiddingView>,RA.Customer.Services.Authenticated.IBiddingView {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIBiddingViewProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IBiddingView> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingView_estateUid_cancellationToken = GetMethodInfo("GetBiddingView", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingView> GetBiddingView(
                System.Guid estateUid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IBiddingView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingView>(_serviceProvider, impl, mi_GetBiddingView_estateUid_cancellationToken, new object[] {estateUid, cancellationToken}, () => impl.GetBiddingView(estateUid, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AddBid_estateUid_bid_cancellationToken = GetMethodInfo("AddBid", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.Bidding.NewBid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="bid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Boolean> AddBid(
                System.Guid estateUid,
                RA.Customer.Types.Bidding.NewBid bid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IBiddingView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Boolean>(_serviceProvider, impl, mi_AddBid_estateUid_bid_cancellationToken, new object[] {estateUid, bid, cancellationToken}, () => impl.AddBid(estateUid, bid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIEstateViewProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IEstateView>,RA.Customer.Services.Authenticated.IEstateView {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIEstateViewProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IEstateView> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetByEstateId_view_uid_cancellation = GetMethodInfo("GetByEstateId", new System.Type[] {typeof(System.String), typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="view"></param>
            /// <param name="uid"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.BuySellEstate> GetByEstateId(
                System.String view,
                System.Guid uid,
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Authenticated.IEstateView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.BuySellEstate>(_serviceProvider, impl, mi_GetByEstateId_view_uid_cancellation, new object[] {view, uid, cancellation}, () => impl.GetByEstateId(view, uid, cancellation));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIHomepageViewProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IHomepageView>,RA.Customer.Services.Authenticated.IHomepageView {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIHomepageViewProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IHomepageView> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetHomepageDataAsync_cancellationToken = GetMethodInfo("GetHomepageDataAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Homepage.HomepageData> GetHomepageDataAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IHomepageView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Homepage.HomepageData>(_serviceProvider, impl, mi_GetHomepageDataAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetHomepageDataAsync(cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedILoanCommitmentInfoProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.ILoanCommitmentInfo>,RA.Customer.Services.Authenticated.ILoanCommitmentInfo {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedILoanCommitmentInfoProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.ILoanCommitmentInfo> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_UpsertLoanCommitmentInfo_estateUid_loanCommitmentInfo_cancellationToken = GetMethodInfo("UpsertLoanCommitmentInfo", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.Bidding.LoanCommitmentInfo), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateUid"></param>
            /// <param name="loanCommitmentInfo"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpsertLoanCommitmentInfo(
                System.Guid estateUid,
                RA.Customer.Types.Bidding.LoanCommitmentInfo loanCommitmentInfo,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.ILoanCommitmentInfo)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpsertLoanCommitmentInfo_estateUid_loanCommitmentInfo_cancellationToken, new object[] {estateUid, loanCommitmentInfo, cancellationToken}, () => impl.UpsertLoanCommitmentInfo(estateUid, loanCommitmentInfo, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIOnboardingProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IOnboarding>,RA.Customer.Services.Authenticated.IOnboarding {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIOnboardingProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IOnboarding> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetOnboardingViewAsync_source_contactId_estateId_cancellationToken = GetMethodInfo("GetOnboardingViewAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="contactId"></param>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Onboarding.OnboardingView> GetOnboardingViewAsync(
                System.String source,
                System.String contactId,
                System.String estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IOnboarding)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Onboarding.OnboardingView>(_serviceProvider, impl, mi_GetOnboardingViewAsync_source_contactId_estateId_cancellationToken, new object[] {source, contactId, estateId, cancellationToken}, () => impl.GetOnboardingViewAsync(source, contactId, estateId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_CorrectUserLoggedInAsync_source_contactId_estateId_cancellationToken = GetMethodInfo("CorrectUserLoggedInAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="contactId"></param>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CorrectUserLoggedInAsync(
                System.String source,
                System.String contactId,
                System.String estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IOnboarding)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CorrectUserLoggedInAsync_source_contactId_estateId_cancellationToken, new object[] {source, contactId, estateId, cancellationToken}, () => impl.CorrectUserLoggedInAsync(source, contactId, estateId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SetContactSsnAsync_source_contactId_cancellationToken = GetMethodInfo("SetContactSsnAsync", new System.Type[] {typeof(System.String), typeof(System.String), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            /// <param name="contactId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Onboarding.Contact> SetContactSsnAsync(
                System.String source,
                System.String contactId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IOnboarding)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Onboarding.Contact>(_serviceProvider, impl, mi_SetContactSsnAsync_source_contactId_cancellationToken, new object[] {source, contactId, cancellationToken}, () => impl.SetContactSsnAsync(source, contactId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIRecommendationProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IRecommendation>,RA.Customer.Services.Authenticated.IRecommendation {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIRecommendationProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IRecommendation> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_SendRecommendation_recommendation_cancellationToken = GetMethodInfo("SendRecommendation", new System.Type[] {typeof(RA.Customer.Types.BuySell.Recommendation), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="recommendation"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task SendRecommendation(
                RA.Customer.Types.BuySell.Recommendation recommendation,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IRecommendation)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_SendRecommendation_recommendation_cancellationToken, new object[] {recommendation, cancellationToken}, () => impl.SendRecommendation(recommendation, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_ProcessRecommendation_guid_cancellationToken = GetMethodInfo("ProcessRecommendation", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="guid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task ProcessRecommendation(
                System.Guid guid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IRecommendation)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_ProcessRecommendation_guid_cancellationToken, new object[] {guid, cancellationToken}, () => impl.ProcessRecommendation(guid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIUserDataProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IUserData>,RA.Customer.Services.Authenticated.IUserData {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIUserDataProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IUserData> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetCurrentUserAsync_cancellationToken = GetMethodInfo("GetCurrentUserAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.User.UserInfo> GetCurrentUserAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IUserData)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.User.UserInfo>(_serviceProvider, impl, mi_GetCurrentUserAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetCurrentUserAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetNotificationsChangeTokenAsync_cancellationToken = GetMethodInfo("GetNotificationsChangeTokenAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> GetNotificationsChangeTokenAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IUserData)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_GetNotificationsChangeTokenAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetNotificationsChangeTokenAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_HasUnreadNotificationsAsync_cancellationToken = GetMethodInfo("HasUnreadNotificationsAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Boolean> HasUnreadNotificationsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IUserData)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Boolean>(_serviceProvider, impl, mi_HasUnreadNotificationsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.HasUnreadNotificationsAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetNotificationsAsync_cancellationToken = GetMethodInfo("GetNotificationsAsync", new System.Type[] {typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.User.Notification>> GetNotificationsAsync(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IUserData)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.User.Notification>>(_serviceProvider, impl, mi_GetNotificationsAsync_cancellationToken, new object[] {cancellationToken}, () => impl.GetNotificationsAsync(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_MarkNotificationsAsRead_notificationIds_cancellationToken = GetMethodInfo("MarkNotificationsAsRead", new System.Type[] {typeof(System.Guid[]), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="notificationIds"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task MarkNotificationsAsRead(
                System.Guid[] notificationIds,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IUserData)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_MarkNotificationsAsRead_notificationIds_cancellationToken, new object[] {notificationIds, cancellationToken}, () => impl.MarkNotificationsAsRead(notificationIds, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedIOffersViewProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.IOffersView>,RA.Customer.Services.Authenticated.IOffersView {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedIOffersViewProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.IOffersView> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetOffersForEstate_estateId_cancellationToken = GetMethodInfo("GetOffersForEstate", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<RA.Customer.Types.BuySell.Tip>> GetOffersForEstate(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.IOffersView)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<RA.Customer.Types.BuySell.Tip>>(_serviceProvider, impl, mi_GetOffersForEstate_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetOffersForEstate(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellSellISellAfterEntryProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellAfterEntry>,RA.Customer.Services.Authenticated.BuySell.Sell.ISellAfterEntry {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellSellISellAfterEntryProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Sell.ISellAfterEntry> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSellAfterEntryViewAsync_estateId_cancellation = GetMethodInfo("GetSellAfterEntryViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellation"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Sell.SellAfterEntryView> GetSellAfterEntryViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellation) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Sell.ISellAfterEntry)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Sell.SellAfterEntryView>(_serviceProvider, impl, mi_GetSellAfterEntryViewAsync_estateId_cancellation, new object[] {estateId, cancellation}, () => impl.GetSellAfterEntryViewAsync(estateId, cancellation));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellSellISellContractProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellContract>,RA.Customer.Services.Authenticated.BuySell.Sell.ISellContract {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellSellISellContractProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Sell.ISellContract> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSellContractViewAsync_estateId_cancellationToken = GetMethodInfo("GetSellContractViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Sell.SellContractView> GetSellContractViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Sell.ISellContract)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Sell.SellContractView>(_serviceProvider, impl, mi_GetSellContractViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetSellContractViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellSellISellDownPaymentProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellDownPayment>,RA.Customer.Services.Authenticated.BuySell.Sell.ISellDownPayment {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellSellISellDownPaymentProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Sell.ISellDownPayment> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSellDownPaymentViewAsync_estateId_cancellationToken = GetMethodInfo("GetSellDownPaymentViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Sell.SellDownPaymentView> GetSellDownPaymentViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Sell.ISellDownPayment)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Sell.SellDownPaymentView>(_serviceProvider, impl, mi_GetSellDownPaymentViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetSellDownPaymentViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellSellISellEntryProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellEntry>,RA.Customer.Services.Authenticated.BuySell.Sell.ISellEntry {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellSellISellEntryProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Sell.ISellEntry> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSellEntryViewAsync_estateId_cancellationToken = GetMethodInfo("GetSellEntryViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Sell.SellEntryView> GetSellEntryViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Sell.ISellEntry)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Sell.SellEntryView>(_serviceProvider, impl, mi_GetSellEntryViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetSellEntryViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellSellISellMarketingProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellMarketing>,RA.Customer.Services.Authenticated.BuySell.Sell.ISellMarketing {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellSellISellMarketingProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Sell.ISellMarketing> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSellMarketingViewAsync_estateId_cancellationToken = GetMethodInfo("GetSellMarketingViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Sell.SellMarketingView> GetSellMarketingViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Sell.ISellMarketing)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Sell.SellMarketingView>(_serviceProvider, impl, mi_GetSellMarketingViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetSellMarketingViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellSellISellPreparationsProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellPreparations>,RA.Customer.Services.Authenticated.BuySell.Sell.ISellPreparations {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellSellISellPreparationsProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Sell.ISellPreparations> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSellPreparationsViewAsync_estateId_cancellationToken = GetMethodInfo("GetSellPreparationsViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Sell.SellPreparationsView> GetSellPreparationsViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Sell.ISellPreparations)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Sell.SellPreparationsView>(_serviceProvider, impl, mi_GetSellPreparationsViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetSellPreparationsViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellSellISellViewingBiddingProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellViewingBidding>,RA.Customer.Services.Authenticated.BuySell.Sell.ISellViewingBidding {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellSellISellViewingBiddingProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Sell.ISellViewingBidding> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetSellViewingBiddingViewAsync_estateId_cancellationToken = GetMethodInfo("GetSellViewingBiddingViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Sell.SellViewingBiddingView> GetSellViewingBiddingViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Sell.ISellViewingBidding)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Sell.SellViewingBiddingView>(_serviceProvider, impl, mi_GetSellViewingBiddingViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetSellViewingBiddingViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellBuyIBuyAfterEntryProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyAfterEntry>,RA.Customer.Services.Authenticated.BuySell.Buy.IBuyAfterEntry {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellBuyIBuyAfterEntryProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyAfterEntry> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBuyAfterEntryViewAsync_estateId_cancellationToken = GetMethodInfo("GetBuyAfterEntryViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Buy.BuyAfterEntryView> GetBuyAfterEntryViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Buy.IBuyAfterEntry)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Buy.BuyAfterEntryView>(_serviceProvider, impl, mi_GetBuyAfterEntryViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBuyAfterEntryViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellBuyIBuyContractProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyContract>,RA.Customer.Services.Authenticated.BuySell.Buy.IBuyContract {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellBuyIBuyContractProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyContract> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBuyContractViewAsync_estateId_cancellationToken = GetMethodInfo("GetBuyContractViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Buy.BuyContractView> GetBuyContractViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Buy.IBuyContract)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Buy.BuyContractView>(_serviceProvider, impl, mi_GetBuyContractViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBuyContractViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellBuyIBuyDownPaymentProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyDownPayment>,RA.Customer.Services.Authenticated.BuySell.Buy.IBuyDownPayment {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellBuyIBuyDownPaymentProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyDownPayment> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBuyDownPaymentViewAsync_estateId_cancellationToken = GetMethodInfo("GetBuyDownPaymentViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Buy.BuyDownPaymentView> GetBuyDownPaymentViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Buy.IBuyDownPayment)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Buy.BuyDownPaymentView>(_serviceProvider, impl, mi_GetBuyDownPaymentViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBuyDownPaymentViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBuySellBuyIBuyEntryProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyEntry>,RA.Customer.Services.Authenticated.BuySell.Buy.IBuyEntry {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBuySellBuyIBuyEntryProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyEntry> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBuyEntryViewAsync_estateId_cancellationToken = GetMethodInfo("GetBuyEntryViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.BuySell.Buy.BuyEntryView> GetBuyEntryViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.BuySell.Buy.IBuyEntry)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.BuySell.Buy.BuyEntryView>(_serviceProvider, impl, mi_GetBuyEntryViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBuyEntryViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBiddingIBiddingBidLooserProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.Bidding.IBiddingBidLooser>,RA.Customer.Services.Authenticated.Bidding.IBiddingBidLooser {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBiddingIBiddingBidLooserProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.Bidding.IBiddingBidLooser> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingBidLooserViewAsync_estateId_cancellationToken = GetMethodInfo("GetBiddingBidLooserViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingBidLooserView> GetBiddingBidLooserViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingBidLooser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingBidLooserView>(_serviceProvider, impl, mi_GetBiddingBidLooserViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBiddingBidLooserViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBiddingIBiddingBidWinnerProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.Bidding.IBiddingBidWinner>,RA.Customer.Services.Authenticated.Bidding.IBiddingBidWinner {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBiddingIBiddingBidWinnerProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.Bidding.IBiddingBidWinner> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingBidWinnerViewAsync_estateId_cancellationToken = GetMethodInfo("GetBiddingBidWinnerViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingBidWinnerView> GetBiddingBidWinnerViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingBidWinner)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingBidWinnerView>(_serviceProvider, impl, mi_GetBiddingBidWinnerViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBiddingBidWinnerViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBiddingIBiddingHaltedProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.Bidding.IBiddingHalted>,RA.Customer.Services.Authenticated.Bidding.IBiddingHalted {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBiddingIBiddingHaltedProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.Bidding.IBiddingHalted> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingHaltedViewAsync_estateId_cancellationToken = GetMethodInfo("GetBiddingHaltedViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingHaltedView> GetBiddingHaltedViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingHalted)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingHaltedView>(_serviceProvider, impl, mi_GetBiddingHaltedViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBiddingHaltedViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBiddingIBiddingHighestBidProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.Bidding.IBiddingHighestBid>,RA.Customer.Services.Authenticated.Bidding.IBiddingHighestBid {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBiddingIBiddingHighestBidProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.Bidding.IBiddingHighestBid> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingHighestBidViewAsync_estateId_cancellationToken = GetMethodInfo("GetBiddingHighestBidViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingHighestBidView> GetBiddingHighestBidViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingHighestBid)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingHighestBidView>(_serviceProvider, impl, mi_GetBiddingHighestBidViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBiddingHighestBidViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBiddingIBiddingNotBiddingParticipantProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.Bidding.IBiddingNotBiddingParticipant>,RA.Customer.Services.Authenticated.Bidding.IBiddingNotBiddingParticipant {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBiddingIBiddingNotBiddingParticipantProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.Bidding.IBiddingNotBiddingParticipant> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingNotBiddingParticipantViewAsync_estateId_cancellationToken = GetMethodInfo("GetBiddingNotBiddingParticipantViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingNotBiddingParticipantView> GetBiddingNotBiddingParticipantViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingNotBiddingParticipant)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingNotBiddingParticipantView>(_serviceProvider, impl, mi_GetBiddingNotBiddingParticipantViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBiddingNotBiddingParticipantViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBiddingIBiddingPausedProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.Bidding.IBiddingPaused>,RA.Customer.Services.Authenticated.Bidding.IBiddingPaused {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBiddingIBiddingPausedProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.Bidding.IBiddingPaused> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingPausedViewAsync_estateId_cancellationToken = GetMethodInfo("GetBiddingPausedViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingPausedView> GetBiddingPausedViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingPaused)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingPausedView>(_serviceProvider, impl, mi_GetBiddingPausedViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBiddingPausedViewAsync(estateId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class RACustomerServicesAuthenticatedBiddingIBiddingPlaceBidProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<RA.Customer.Services.Authenticated.Bidding.IBiddingPlaceBid>,RA.Customer.Services.Authenticated.Bidding.IBiddingPlaceBid {
            /// <summary>
            /// 
            /// </summary>
            public RACustomerServicesAuthenticatedBiddingIBiddingPlaceBidProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<RA.Customer.Services.Authenticated.Bidding.IBiddingPlaceBid> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_GetBiddingPlaceBidViewAsync_estateId_cancellationToken = GetMethodInfo("GetBiddingPlaceBidViewAsync", new System.Type[] {typeof(System.Guid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<RA.Customer.Types.Bidding.BiddingPlaceBidView> GetBiddingPlaceBidViewAsync(
                System.Guid estateId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingPlaceBid)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<RA.Customer.Types.Bidding.BiddingPlaceBidView>(_serviceProvider, impl, mi_GetBiddingPlaceBidViewAsync_estateId_cancellationToken, new object[] {estateId, cancellationToken}, () => impl.GetBiddingPlaceBidViewAsync(estateId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            private static System.Reflection.MethodInfo mi_AddBid_estateId_bid_cancellationToken = GetMethodInfo("AddBid", new System.Type[] {typeof(System.Guid), typeof(RA.Customer.Types.Bidding.NewBid), typeof(System.Threading.CancellationToken)});
            /// <summary>
            /// 
            /// </summary>
            /// <param name="estateId"></param>
            /// <param name="bid"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.Boolean> AddBid(
                System.Guid estateId,
                RA.Customer.Types.Bidding.NewBid bid,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (RA.Customer.Services.Authenticated.Bidding.IBiddingPlaceBid)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Boolean>(_serviceProvider, impl, mi_AddBid_estateId_bid_cancellationToken, new object[] {estateId, bid, cancellationToken}, () => impl.AddBid(estateId, bid, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddRACustomer(
            this Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<RA.Customer.Services.IBoostadAdmin,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesIBoostadAdminProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.IImpersonate,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesIImpersonateProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.ISettings,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesISettingsProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Public.IApplication,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesPublicIApplicationProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Public.IBankId,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesPublicIBankIdProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IAdvertise,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIAdvertiseProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IBroker,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIBrokerProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IChecklist,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIChecklistProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IContact,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIContactProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IEstate,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIEstateProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IEstateStatistics,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIEstateStatisticsProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IEstateStatisticsGoogle,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIEstateStatisticsGoogleProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IMspecs,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIMspecsProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.INotification,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendINotificationProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IOffice,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIOfficeProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IResources,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIResourcesProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.ISettings,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendISettingsProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.ISPBolan,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendISPBolanProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IStep,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIStepProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.ITheme,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIThemeProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.ITinyUrl,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendITinyUrlProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IVitec,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIVitecProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Backend.IVitecUpdate,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesBackendIVitecUpdateProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IChecklistView,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIChecklistViewProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IDocumentView,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIDocumentViewProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IBiddingView,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIBiddingViewProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IEstateView,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIEstateViewProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IHomepageView,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIHomepageViewProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.ILoanCommitmentInfo,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedILoanCommitmentInfoProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IOnboarding,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIOnboardingProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IRecommendation,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIRecommendationProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IUserData,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIUserDataProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.IOffersView,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedIOffersViewProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellAfterEntry,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellSellISellAfterEntryProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellContract,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellSellISellContractProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellDownPayment,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellSellISellDownPaymentProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellEntry,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellSellISellEntryProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellMarketing,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellSellISellMarketingProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellPreparations,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellSellISellPreparationsProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Sell.ISellViewingBidding,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellSellISellViewingBiddingProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyAfterEntry,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellBuyIBuyAfterEntryProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyContract,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellBuyIBuyContractProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyDownPayment,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellBuyIBuyDownPaymentProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.BuySell.Buy.IBuyEntry,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBuySellBuyIBuyEntryProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.Bidding.IBiddingBidLooser,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBiddingIBiddingBidLooserProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.Bidding.IBiddingBidWinner,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBiddingIBiddingBidWinnerProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.Bidding.IBiddingHalted,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBiddingIBiddingHaltedProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.Bidding.IBiddingHighestBid,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBiddingIBiddingHighestBidProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.Bidding.IBiddingNotBiddingParticipant,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBiddingIBiddingNotBiddingParticipantProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.Bidding.IBiddingPaused,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBiddingIBiddingPausedProxy>(configure);
            sc.SetupProxy<RA.Customer.Services.Authenticated.Bidding.IBiddingPlaceBid,Microsoft.Extensions.DependencyInjection.RACustomerExtensions.RACustomerServicesAuthenticatedBiddingIBiddingPlaceBidProxy>(configure);
            return sc;
        }
    
    }
}