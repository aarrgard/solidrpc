﻿using NUnit.Framework;
using SolidRpc.Swagger.Model;
using SolidRpc.Swagger.Model.V2;
using SolidRpc.Tests.Swagger.Petstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidRpc.Tests.Swagger
{
    public class SwaggerTest : TestBase
    {
        [Test]
        public void TestPetStore()
        {
            var swaggerSpec = new SwaggerParserV2().ParseSwaggerDoc(GetManifestResource("petstore.json"));

            CheckPetStoreSwaggerSpec(swaggerSpec);

            var str = new SwaggerParserV2().WriteSwaggerDoc(swaggerSpec);
            swaggerSpec = new SwaggerParserV2().ParseSwaggerDoc(str);

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
            Assert.AreEqual("string", swaggerSpec.Definitions["Category"].Properties["Name"].Type);
            Assert.AreEqual("Category", swaggerSpec.Definitions["Category"].Xml.Name);
        }

        [Test]
        public void TestMethodBinder()
        {
            var swaggerSpec = new SwaggerParserV2().ParseSwaggerDoc(GetManifestResource("petstore.json"));

            var mi = typeof(IPet).GetMethod(nameof(IPet.FindPetsByStatus));
            var smi = swaggerSpec.GetMethodBinder().GetMethodInfo(mi);
            Assert.AreEqual("findPetsByStatus", smi.OperationId);
            Assert.AreEqual(1, smi.Arguments.Count());
            Assert.IsNotNull(smi.Arguments.Single(o => o.Name == "status"));

            var req = new RequestMock();
            smi.BindArguments(req, new object[]{ new string[] { "available", "pending" } });
            var status = req.Query["status"];
            Assert.AreEqual("available", status.First());
            Assert.AreEqual("pending", status.Last());
        }
    }
}
