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
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task AddPet(
                SolidRpc.Test.Petstore.Types.Pet body,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().AddPet(body, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task UpdatePet(
                SolidRpc.Test.Petstore.Types.Pet body,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().UpdatePet(body, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="status"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.Pet>> FindPetsByStatus(
                System.Collections.Generic.IEnumerable<System.String> status,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().FindPetsByStatus(status, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="tags"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.PetstoreAuth(Scopes=new [] {"write:pets","read:pets"})]
            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.Pet>> FindPetsByTags(
                System.Collections.Generic.IEnumerable<System.String> tags,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().FindPetsByTags(tags, cancellationToken);
            }
        
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
                return GetImplementation().DeletePet(petId, apiKey, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="petId"></param>
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.ApiKey]
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Pet> GetPetById(
                System.Int64 petId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetPetById(petId, cancellationToken);
            }
        
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
                return GetImplementation().UpdatePetWithForm(petId, name, status, cancellationToken);
            }
        
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
                return GetImplementation().UploadFile(petId, additionalMetadata, file, cancellationToken);
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
            /// <param name="cancellationToken"></param>
            [SolidRpc.Test.Petstore.Security.ApiKey]
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Services.Store.GetInventory.Response200> GetInventory(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetInventory(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Order> PlaceOrder(
                SolidRpc.Test.Petstore.Types.Order body,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().PlaceOrder(body, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="orderId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteOrder(
                System.Int64 orderId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().DeleteOrder(orderId, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="orderId"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.Order> GetOrderById(
                System.Int64 orderId,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetOrderById(orderId, cancellationToken);
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
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CreateUser(
                SolidRpc.Test.Petstore.Types.User body,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateUser(body, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CreateUsersWithArrayInput(
                System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.User> body,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateUsersWithArrayInput(body, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="body"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task CreateUsersWithListInput(
                System.Collections.Generic.IEnumerable<SolidRpc.Test.Petstore.Types.User> body,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().CreateUsersWithListInput(body, cancellationToken);
            }
        
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
                return GetImplementation().LoginUser(username, password, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task LogoutUser(
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().LogoutUser(cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task DeleteUser(
                System.String username,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().DeleteUser(username, cancellationToken);
            }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="cancellationToken"></param>
            public System.Threading.Tasks.Task<SolidRpc.Test.Petstore.Types.User> GetUserByName(
                System.String username,
                System.Threading.CancellationToken cancellationToken) {
                return GetImplementation().GetUserByName(username, cancellationToken);
            }
        
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
                return GetImplementation().UpdateUser(username, body, cancellationToken);
            }
        
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="configure"></param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSolidRpcTestPetstore(
            Microsoft.Extensions.DependencyInjection.IServiceCollection sc,
            System.Func<Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig,Microsoft.Extensions.DependencyInjection.SolidRpcExtensions.IProxyConfig> configure) {
            sc.SetupProxy<SolidRpc.Test.Petstore.Services.IPet,Microsoft.Extensions.DependencyInjection.SolidRpcTestPetstoreExtensions.SolidRpcTestPetstoreServicesIPetProxy>(configure);
            sc.SetupProxy<SolidRpc.Test.Petstore.Services.IStore,Microsoft.Extensions.DependencyInjection.SolidRpcTestPetstoreExtensions.SolidRpcTestPetstoreServicesIStoreProxy>(configure);
            sc.SetupProxy<SolidRpc.Test.Petstore.Services.IUser,Microsoft.Extensions.DependencyInjection.SolidRpcTestPetstoreExtensions.SolidRpcTestPetstoreServicesIUserProxy>(configure);
            return sc;
        }
    
    }
}