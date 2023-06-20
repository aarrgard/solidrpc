namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// 
    /// </summary>
    public static class SolidRpcTestPetstoreExtensions {
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcTestPetstoreServicesIPetProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Test.Petstore.Services.IPet>,SolidRpc.Test.Petstore.Services.IPet {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcTestPetstoreServicesIPetProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Test.Petstore.Services.IPet> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_AddPet_body_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task AddPet(
                SolidRpc.Test.Petstore.Types.Pet body,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_AddPet_body_cancellationToken, new object[] {body, cancellationToken}, () => impl.AddPet(body, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpdatePet_body_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task UpdatePet(
                SolidRpc.Test.Petstore.Types.Pet body,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdatePet_body_cancellationToken, new object[] {body, cancellationToken}, () => impl.UpdatePet(body, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_FindPetsByStatus_status_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="status"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.Pet>> FindPetsByStatus(
                System.Collections.Generic.IEnumerable<System.String> status,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.Pet>>(_serviceProvider, impl, mi_FindPetsByStatus_status_cancellationToken, new object[] {status, cancellationToken}, () => impl.FindPetsByStatus(status, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_FindPetsByTags_tags_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="tags"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.Pet>> FindPetsByTags(
                System.Collections.Generic.IEnumerable<System.String> tags,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.Pet>>(_serviceProvider, impl, mi_FindPetsByTags_tags_cancellationToken, new object[] {tags, cancellationToken}, () => impl.FindPetsByTags(tags, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_DeletePet_petId_apiKey_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="apiKey"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task DeletePet(
                System.Int64 petId,
                System.String apiKey,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DeletePet_petId_apiKey_cancellationToken, new object[] {petId, apiKey, cancellationToken}, () => impl.DeletePet(petId, apiKey, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetPetById_petId_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.ApiKey]
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Pet> GetPetById(
                System.Int64 petId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Test.Petstore.Types.Pet>(_serviceProvider, impl, mi_GetPetById_petId_cancellationToken, new object[] {petId, cancellationToken}, () => impl.GetPetById(petId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpdatePetWithForm_petId_name_status_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="name"></param>
            /// <param name="status"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task UpdatePetWithForm(
                System.Int64 petId,
                System.String name,
                System.String status,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdatePetWithForm_petId_name_status_cancellationToken, new object[] {petId, name, status, cancellationToken}, () => impl.UpdatePetWithForm(petId, name, status, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UploadFile_petId_additionalMetadata_file_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="additionalMetadata"></param>
            /// <param name="file"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.ApiResponse> UploadFile(
                System.Int64 petId,
                System.String additionalMetadata,
                System.IO.Stream file,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IPet)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Test.Petstore.Types.ApiResponse>(_serviceProvider, impl, mi_UploadFile_petId_additionalMetadata_file_cancellationToken, new object[] {petId, additionalMetadata, file, cancellationToken}, () => impl.UploadFile(petId, additionalMetadata, file, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcTestPetstoreServicesIStoreProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Test.Petstore.Services.IStore>,SolidRpc.Test.Petstore.Services.IStore {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcTestPetstoreServicesIStoreProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Test.Petstore.Services.IStore> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetInventory_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.ApiKey]
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Services.Store.GetInventory.Response200> GetInventory(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Test.Petstore.Types.Services.Store.GetInventory.Response200>(_serviceProvider, impl, mi_GetInventory_cancellationToken, new object[] {cancellationToken}, () => impl.GetInventory(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_PlaceOrder_body_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Order> PlaceOrder(
                SolidRpc.Test.Petstore.Types.Order body,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Test.Petstore.Types.Order>(_serviceProvider, impl, mi_PlaceOrder_body_cancellationToken, new object[] {body, cancellationToken}, () => impl.PlaceOrder(body, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_DeleteOrder_orderId_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="orderId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteOrder(
                System.Int64 orderId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DeleteOrder_orderId_cancellationToken, new object[] {orderId, cancellationToken}, () => impl.DeleteOrder(orderId, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetOrderById_orderId_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="orderId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Order> GetOrderById(
                System.Int64 orderId,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IStore)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Test.Petstore.Types.Order>(_serviceProvider, impl, mi_GetOrderById_orderId_cancellationToken, new object[] {orderId, cancellationToken}, () => impl.GetOrderById(orderId, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        public class SolidRpcTestPetstoreServicesIUserProxy : Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.Proxy<SolidRpc.Test.Petstore.Services.IUser>,SolidRpc.Test.Petstore.Services.IUser {
            /// <summary>
            /// 
            /// </summary>
            public SolidRpcTestPetstoreServicesIUserProxy(
                System.IServiceProvider serviceProvider,
                Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig<SolidRpc.Test.Petstore.Services.IUser> config) : base(serviceProvider, config)
            {}
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_CreateUser_body_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CreateUser(
                SolidRpc.Test.Petstore.Types.User body,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CreateUser_body_cancellationToken, new object[] {body, cancellationToken}, () => impl.CreateUser(body, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_CreateUsersWithArrayInput_body_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CreateUsersWithArrayInput(
                System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.User> body,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CreateUsersWithArrayInput_body_cancellationToken, new object[] {body, cancellationToken}, () => impl.CreateUsersWithArrayInput(body, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_CreateUsersWithListInput_body_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CreateUsersWithListInput(
                System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.User> body,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_CreateUsersWithListInput_body_cancellationToken, new object[] {body, cancellationToken}, () => impl.CreateUsersWithListInput(body, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_LoginUser_username_password_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="password"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<System.String> LoginUser(
                System.String username,
                System.String password,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<System.String>(_serviceProvider, impl, mi_LoginUser_username_password_cancellationToken, new object[] {username, password, cancellationToken}, () => impl.LoginUser(username, password, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_LogoutUser_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task LogoutUser(
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_LogoutUser_cancellationToken, new object[] {cancellationToken}, () => impl.LogoutUser(cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_DeleteUser_username_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteUser(
                System.String username,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_DeleteUser_username_cancellationToken, new object[] {username, cancellationToken}, () => impl.DeleteUser(username, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_GetUserByName_username_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.User> GetUserByName(
                System.String username,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync<SolidRpc.Test.Petstore.Types.User>(_serviceProvider, impl, mi_GetUserByName_username_cancellationToken, new object[] {username, cancellationToken}, () => impl.GetUserByName(username, cancellationToken));
            }
        
            /// <summary>
            /// 
            /// </summary>
            System.Reflection.MethodInfo mi_UpdateUser_username_body_cancellationToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task UpdateUser(
                System.String username,
                SolidRpc.Test.Petstore.Types.User body,
                System.Threading.CancellationToken cancellationToken) {
                var impl = (SolidRpc.Test.Petstore.Services.IUser)_serviceProvider.GetRequiredService(_config.Implementation ?? throw new System.Exception($"No implementation registered for service {_config.ProxyType.FullName}"));
                return _config.InterceptAsync(_serviceProvider, impl, mi_UpdateUser_username_body_cancellationToken, new object[] {username, body, cancellationToken}, () => impl.UpdateUser(username, body, cancellationToken));
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSolidRpcTestPetstore(
            this Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<SolidRpc.Test.Petstore.Services.IPet,Microsoft.Extensions.DependencyInjection.SolidRpcTestPetstoreExtensions.SolidRpcTestPetstoreServicesIPetProxy>(configure);
            sc.SetupProxy<SolidRpc.Test.Petstore.Services.IStore,Microsoft.Extensions.DependencyInjection.SolidRpcTestPetstoreExtensions.SolidRpcTestPetstoreServicesIStoreProxy>(configure);
            sc.SetupProxy<SolidRpc.Test.Petstore.Services.IUser,Microsoft.Extensions.DependencyInjection.SolidRpcTestPetstoreExtensions.SolidRpcTestPetstoreServicesIUserProxy>(configure);
            return sc;
        }
    
    }
}