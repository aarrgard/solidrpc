using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Services;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Types;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class SwaggerTest : TestBase
    {
        /// <summary>
        /// Tests that the petstore json file is processed correctly.
        /// </summary>
        [Test]
        public void TestPetStore()
        {
            var swaggerSpec = new OpenApiParserV2().ParseSwaggerDoc(GetManifestResource("petstore.json"));

            CheckPetStoreSwaggerSpec(swaggerSpec);

            var str = OpenApiParserV2.WriteSwaggerDoc(swaggerSpec);
            swaggerSpec = new OpenApiParserV2().ParseSwaggerDoc(str);

            CheckPetStoreSwaggerSpec(swaggerSpec);
        }

        private void CheckPetStoreSwaggerSpec(SwaggerObject swaggerSpec)
        {
            // basic tests
            Assert.AreEqual("2.0", swaggerSpec.Swagger);
            Assert.AreEqual("This is a sample Petstore server.  You can find \nout more about Swagger at \n[http://swagger.io](http://swagger.io) or on \n[irc.freenode.net, #swagger](http://swagger.io/irc/).\n", swaggerSpec.Info.Description);
            Assert.AreEqual("1.0.0", swaggerSpec.Info.Version);
            Assert.AreEqual("Swagger Petstore", swaggerSpec.Info.Title);
            Assert.AreEqual("http://swagger.io/terms/", swaggerSpec.Info.TermsOfService);
            Assert.AreEqual("apiteam@swagger.io", swaggerSpec.Info.Contact.Email);
            Assert.AreEqual("Apache 2.0", swaggerSpec.Info.License.Name);
            Assert.AreEqual("http://www.apache.org/licenses/LICENSE-2.0.html", swaggerSpec.Info.License.Url);
            Assert.AreEqual("virtserver.swaggerhub.com", swaggerSpec.Host);
            Assert.AreEqual("/aarrgard/Test/1.0.0", swaggerSpec.BasePath);
            Assert.AreEqual(3, swaggerSpec.Tags.Count());
            // tags
            Assert.AreEqual("pet", swaggerSpec.Tags.Skip(0).First().Name);
            Assert.AreEqual("Everything about your Pets", swaggerSpec.Tags.Skip(0).First().Description);
            Assert.AreEqual("Find out more", swaggerSpec.Tags.Skip(0).First().ExternalDocs.Description);
            Assert.AreEqual("http://swagger.io", swaggerSpec.Tags.Skip(0).First().ExternalDocs.Url);
            Assert.AreEqual("store", swaggerSpec.Tags.Skip(1).First().Name);
            Assert.AreEqual("Access to Petstore orders", swaggerSpec.Tags.Skip(1).First().Description);
            Assert.IsNull(swaggerSpec.Tags.Skip(1).First().ExternalDocs);
            Assert.AreEqual("user", swaggerSpec.Tags.Skip(2).First().Name);
            Assert.AreEqual("Operations about user", swaggerSpec.Tags.Skip(2).First().Description);
            Assert.AreEqual("Find out more about our store", swaggerSpec.Tags.Skip(2).First().ExternalDocs.Description);
            Assert.AreEqual("http://swagger.io", swaggerSpec.Tags.Skip(2).First().ExternalDocs.Url);
            // schemes
            Assert.AreEqual(2, swaggerSpec.Schemes.Length);
            Assert.IsTrue(swaggerSpec.Schemes.Contains("http"));
            Assert.IsTrue(swaggerSpec.Schemes.Contains("https"));
            // paths
            Assert.AreEqual(14, swaggerSpec.Paths.Count);

            Assert.AreEqual("pet", swaggerSpec.Paths["/pet"].Post.Tags.Single());
            Assert.AreEqual("Add a new pet to the store", swaggerSpec.Paths["/pet"].Post.Summary);
            Assert.AreEqual("addPet", swaggerSpec.Paths["/pet"].Post.OperationId);
            Assert.AreEqual("application/json", swaggerSpec.Paths["/pet"].Post.Consumes.First());
            Assert.AreEqual("application/xml", swaggerSpec.Paths["/pet"].Post.Consumes.Last());
            Assert.AreEqual("application/json", swaggerSpec.Paths["/pet"].Post.Produces.First());
            Assert.AreEqual("application/xml", swaggerSpec.Paths["/pet"].Post.Produces.Last());
            Assert.AreEqual(1, swaggerSpec.Paths["/pet"].Post.Parameters.Count());
            Assert.AreEqual("body", swaggerSpec.Paths["/pet"].Post.Parameters.Single().In);
            Assert.AreEqual("body", swaggerSpec.Paths["/pet"].Post.Parameters.Single().Name);
            Assert.AreEqual("Pet object that needs to be added to the store", swaggerSpec.Paths["/pet"].Post.Parameters.Single().Description);
            Assert.AreEqual(true, swaggerSpec.Paths["/pet"].Post.Parameters.Single().Required);
            Assert.AreEqual("#/definitions/Pet", swaggerSpec.Paths["/pet"].Post.Parameters.Single().Schema.Ref);
            Assert.AreEqual("Invalid input", swaggerSpec.Paths["/pet"].Post.Responses["405"].Description);
            Assert.AreEqual("write:pets", swaggerSpec.Paths["/pet"].Post.Security.First()["petstore_auth"].First());
            Assert.AreEqual("read:pets", swaggerSpec.Paths["/pet"].Post.Security.First()["petstore_auth"].Last());

            Assert.AreEqual("pet", swaggerSpec.Paths["/pet"].Post.Tags.Single());
            Assert.AreEqual("Update an existing pet", swaggerSpec.Paths["/pet"].Put.Summary);
            Assert.AreEqual("updatePet", swaggerSpec.Paths["/pet"].Put.OperationId);
            Assert.AreEqual("application/json", swaggerSpec.Paths["/pet"].Put.Consumes.First());
            Assert.AreEqual("application/xml", swaggerSpec.Paths["/pet"].Put.Consumes.Last());
            Assert.AreEqual("application/json", swaggerSpec.Paths["/pet"].Put.Produces.First());
            Assert.AreEqual("application/xml", swaggerSpec.Paths["/pet"].Put.Produces.Last());
            Assert.AreEqual(1, swaggerSpec.Paths["/pet"].Put.Parameters.Count());
            Assert.AreEqual("body", swaggerSpec.Paths["/pet"].Put.Parameters.Single().In);
            Assert.AreEqual("body", swaggerSpec.Paths["/pet"].Put.Parameters.Single().Name);
            Assert.AreEqual("Pet object that needs to be added to the store", swaggerSpec.Paths["/pet"].Put.Parameters.Single().Description);
            Assert.AreEqual(true, swaggerSpec.Paths["/pet"].Put.Parameters.Single().Required);
            //Assert.AreEqual("#/definitions/Pet", swaggerSpec.Paths["/pet"].Post.Parameters.Single().Schema.Ref);
            Assert.AreEqual("Invalid ID supplied", swaggerSpec.Paths["/pet"].Put.Responses["400"].Description);
            Assert.AreEqual("Pet not found", swaggerSpec.Paths["/pet"].Put.Responses["404"].Description);
            Assert.AreEqual("Validation exception", swaggerSpec.Paths["/pet"].Put.Responses["405"].Description);
            Assert.AreEqual("write:pets", swaggerSpec.Paths["/pet"].Put.Security.First()["petstore_auth"].First());
            Assert.AreEqual("read:pets", swaggerSpec.Paths["/pet"].Put.Security.First()["petstore_auth"].Last());

            // ...

            Assert.AreEqual("oauth2", swaggerSpec.SecurityDefinitions["petstore_auth"].Type);
            Assert.AreEqual("http://petstore.swagger.io/oauth/dialog", swaggerSpec.SecurityDefinitions["petstore_auth"].AuthorizationUrl);
            Assert.AreEqual("implicit", swaggerSpec.SecurityDefinitions["petstore_auth"].Flow);
            Assert.AreEqual("modify pets in your account", swaggerSpec.SecurityDefinitions["petstore_auth"].Scopes["write:pets"]);
            Assert.AreEqual("read your pets", swaggerSpec.SecurityDefinitions["petstore_auth"].Scopes["read:pets"]);

            Assert.AreEqual("http://swagger.io", swaggerSpec.ExternalDocs.Url);
            Assert.AreEqual("Find out more about Swagger", swaggerSpec.ExternalDocs.Description);

            // definitions
            Assert.AreEqual(6, swaggerSpec.Definitions.Count);
            // Order
            Assert.AreEqual("object", swaggerSpec.Definitions["Order"].Type);
            Assert.AreEqual(6, swaggerSpec.Definitions["Order"].Properties.Count);
            Assert.AreEqual("integer", swaggerSpec.Definitions["Order"].Properties["id"].Type);
            Assert.AreEqual("int64", swaggerSpec.Definitions["Order"].Properties["id"].Format);
            Assert.AreEqual("integer", swaggerSpec.Definitions["Order"].Properties["petId"].Type);
            Assert.AreEqual("int64", swaggerSpec.Definitions["Order"].Properties["petId"].Format);
            Assert.AreEqual("integer", swaggerSpec.Definitions["Order"].Properties["quantity"].Type);
            Assert.AreEqual("int32", swaggerSpec.Definitions["Order"].Properties["quantity"].Format);
            Assert.AreEqual("string", swaggerSpec.Definitions["Order"].Properties["shipDate"].Type);
            Assert.AreEqual("date-time", swaggerSpec.Definitions["Order"].Properties["shipDate"].Format);
            Assert.AreEqual("string", swaggerSpec.Definitions["Order"].Properties["status"].Type);
            Assert.AreEqual("Order Status", swaggerSpec.Definitions["Order"].Properties["status"].Description);
            Assert.AreEqual("placed", swaggerSpec.Definitions["Order"].Properties["status"].Enum.First());
            Assert.AreEqual("approved", swaggerSpec.Definitions["Order"].Properties["status"].Enum.Skip(1).First());
            Assert.AreEqual("delivered", swaggerSpec.Definitions["Order"].Properties["status"].Enum.Skip(2).First());
            Assert.AreEqual("boolean", swaggerSpec.Definitions["Order"].Properties["complete"].Type);
            Assert.AreEqual(false, swaggerSpec.Definitions["Order"].Properties["complete"].Default);
            // Category
            Assert.AreEqual("object", swaggerSpec.Definitions["Category"].Type);
            Assert.AreEqual(2, swaggerSpec.Definitions["Category"].Properties.Count);
            Assert.AreEqual("integer", swaggerSpec.Definitions["Category"].Properties["id"].Type);
            Assert.AreEqual("int64", swaggerSpec.Definitions["Category"].Properties["id"].Format);
            Assert.AreEqual("string", swaggerSpec.Definitions["Category"].Properties["name"].Type);
            Assert.AreEqual("Category", swaggerSpec.Definitions["Category"].Xml.Name);
            //...
            // Pet
            Assert.AreEqual("object", swaggerSpec.Definitions["Pet"].Type);
            Assert.AreEqual("name", swaggerSpec.Definitions["Pet"].Required.First());
            Assert.AreEqual("photoUrls", swaggerSpec.Definitions["Pet"].Required.Last());
            Assert.AreEqual("integer", swaggerSpec.Definitions["Pet"].Properties["id"].Type);
            Assert.AreEqual("int64", swaggerSpec.Definitions["Pet"].Properties["id"].Format);
            Assert.AreEqual("#/definitions/Category", swaggerSpec.Definitions["Pet"].Properties["category"].Ref);
            Assert.AreEqual("string", swaggerSpec.Definitions["Pet"].Properties["name"].Type);
            Assert.AreEqual("doggie", swaggerSpec.Definitions["Pet"].Properties["name"].Example);
            Assert.AreEqual("array", swaggerSpec.Definitions["Pet"].Properties["photoUrls"].Type);
            Assert.AreEqual("photoUrl", swaggerSpec.Definitions["Pet"].Properties["photoUrls"].Xml.Name);
            Assert.AreEqual(true, swaggerSpec.Definitions["Pet"].Properties["photoUrls"].Xml.Wrapped);
            Assert.AreEqual("string", swaggerSpec.Definitions["Pet"].Properties["photoUrls"].Items.Type);
            Assert.AreEqual("array", swaggerSpec.Definitions["Pet"].Properties["tags"].Type);
            Assert.AreEqual("tag", swaggerSpec.Definitions["Pet"].Properties["tags"].Xml.Name);
            Assert.AreEqual(true, swaggerSpec.Definitions["Pet"].Properties["tags"].Xml.Wrapped);
            Assert.AreEqual("#/definitions/Tag", swaggerSpec.Definitions["Pet"].Properties["tags"].Items.Ref);
        }

        private Uri TransformUri(IServiceProvider serviceProvider, Uri baseUri)
        {
            return baseUri;
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task TestMethodBinderFindPetsByStatus()
        {
            var methodBinderStore = GetMethodBinderStore();
            var mi = typeof(IPet).GetMethod(nameof(IPet.FindPetsByStatus));
            var smi = methodBinderStore.GetMethodInfo(GetManifestResourceAsString("petstore.json"), mi);
            Assert.AreEqual("findPetsByStatus", smi.OperationId);
            Assert.AreEqual(2, smi.Arguments.Count());
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "status"));

            var req = new SolidHttpRequest();
            await smi.BindArgumentsAsync(req, new object[] { new string[] { "available", "pending" }, CancellationToken.None });
            Assert.AreEqual("GET", req.Method);
            Assert.AreEqual("/aarrgard/Test/1.0.0/pet/findByStatus", req.Path);
            Assert.AreEqual("available", req.Query.As<string>("status").First());
            Assert.AreEqual("pending", req.Query.As<string>("status").Last());
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task TestMethodBinderGetPetById()
        {
            var methodBinderStore = GetMethodBinderStore();

            var mi = typeof(IPet).GetMethod(nameof(IPet.GetPetById));
            var smi = methodBinderStore.GetMethodInfo(GetManifestResourceAsString("petstore.json"), mi);
            Assert.AreEqual("getPetById", smi.OperationId);
            Assert.AreEqual(2, smi.Arguments.Count());
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "petId"));

            var petId = 3456L;
            var req = new SolidHttpRequest();
            await smi.BindArgumentsAsync(req, new object[] { petId, CancellationToken.None });
            Assert.AreEqual("GET", req.Method);
            Assert.AreEqual($"/aarrgard/Test/1.0.0/pet/{petId}",req.Path);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task TestMethodBinderUpdatePetWithForm()
        {
            var methodBinderStore = GetMethodBinderStore();
            var mi = typeof(IPet).GetMethod(nameof(IPet.UpdatePetWithForm));
            var smi = methodBinderStore.GetMethodInfo(GetManifestResourceAsString("petstore.json"), mi);
            Assert.AreEqual("updatePetWithForm", smi.OperationId);
            Assert.AreEqual(4, smi.Arguments.Count());
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "petId"));
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "name"));
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "status"));

            var petId = 3456L;
            var req = new SolidHttpRequest();
            await smi.BindArgumentsAsync(req, new object[] { petId, "kalle", "available", CancellationToken.None });
            Assert.AreEqual("POST", req.Method);
            Assert.AreEqual($"/aarrgard/Test/1.0.0/pet/{petId}", req.Path);
            Assert.AreEqual($"kalle", req.BodyData.As<string>("name").First());
            Assert.AreEqual($"available", req.BodyData.As<string>("status").First());
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task TestMethodBinderUploadFile()
        {
            var methodBinderStore = GetMethodBinderStore();
            var mi = typeof(IPet).GetMethod(nameof(IPet.UploadFile));
            var smi = methodBinderStore.GetMethodInfo(GetManifestResourceAsString("petstore.json"), mi);
            Assert.AreEqual("uploadFile", smi.OperationId);
            Assert.AreEqual(4, smi.Arguments.Count());
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "petId"));
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "additionalMetadata"));
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "file"));

            var petId = 3456L;
            var req = new SolidHttpRequest();
            var data = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            await smi.BindArgumentsAsync(req, new object[] { petId, "Image metadata", data, CancellationToken.None });
            Assert.AreEqual("POST", req.Method);
            Assert.AreEqual($"/aarrgard/Test/1.0.0/pet/{petId}/uploadImage", req.Path);
            Assert.AreEqual($"Image metadata", req.BodyData.As<string>("additionalMetadata").First());
            Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, req.BodyData.GetData<byte[]>("file"));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task TestMethodBinderPlaceOrder()
        {
            var methodBinderStore = GetMethodBinderStore();
            var mi = typeof(IStore).GetMethod(nameof(IStore.PlaceOrder));
            var smi = methodBinderStore.GetMethodInfo(GetManifestResourceAsString("petstore.json"), mi);
            Assert.AreEqual("placeOrder", smi.OperationId);
            Assert.AreEqual(2, smi.Arguments.Count());
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "body"));

            var req = new SolidHttpRequest();
            var order = new Order()
            {
                Id = 343,
                PetId = 34234,
                Quantity = 1,
                ShipDate = DateTime.Now,
                Status = "sdfsdf",
                Complete = false
            };

            await smi.BindArgumentsAsync(req, new object[] { order, CancellationToken.None });
            Assert.AreEqual("POST", req.Method);
            Assert.AreEqual($"/aarrgard/Test/1.0.0/store/order", req.Path);
            var orderResp = req.BodyData.First().As<Order>();
            Assert.AreEqual(order.Complete, orderResp.Complete);
            Assert.AreEqual(order.Status, orderResp.Status);
            Assert.AreEqual(order.Quantity, orderResp.Quantity);
            Assert.AreEqual(order.PetId, orderResp.PetId);
            Assert.AreEqual(order.Id, orderResp.Id);
        }

        private IMethodBinderStore GetMethodBinderStore()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSingletonServices();
            return sc.BuildServiceProvider().GetRequiredService<IMethodBinderStore>();
        }
    }
}
