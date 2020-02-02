using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SolidRpc.OpenApi.DotNetTool;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.User.UpdateUser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.CodeGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class PetStoreTest : WebHostTest
    {
        /// <summary>
        /// Returns the spec folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected override DirectoryInfo GetSpecFolder(string folderName)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger", "CodeGen", folderName);
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestPetstoreUsingKestrel()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await TestPetstore(ctx);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestPetstoreUsingHttpMessageHandler()
        {
            using (var ctx = CreateHttpMessageHandlerContext())
            {
                await TestPetstore(ctx);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        public async Task TestPetstore(TestHostContext ctx)
        {
            var config = ReadOpenApiConfiguration(nameof(TestPetstore).Substring(4));
            var api_key = "sfasdfoewr0sdf";
            var pet = new Petstore.Types.Pet()
            {
                Status = "test",
                Id = 22314,
                Category = new Petstore.Types.Category()
                {
                    Id = 334,
                    Name = "test"
                },
                Name = "Kalle",
                Tags = new Petstore.Types.Tag[] {
                    new Petstore.Types.Tag()
                    {
                        Id = 23123,
                        Name = "Tag1"
                    }
                },
                PhotoUrls = new string[] { "url1", "url2" }
            };
            var statuses = new string[] { "test1", "test2"};
            var tags = new Petstore.Types.Tag[] {
                new Petstore.Types.Tag() { Id = 1 , Name = "Tag name 1"},
                new Petstore.Types.Tag() { Id = 2 , Name = "Tag name 2"}
            };
            var apiResponse = new Petstore.Types.ApiResponse()
            {
                Code = 200,
                Message = "Test message",
                Type = "message type"
            };

            var order = new Petstore.Types.Order()
            {
                Id = 1234,
                PetId = 234,
                ShipDate = new DateTime(2018, 12, 01),
                Complete = false,
                Quantity = 3,
                Status = "pending"
            };

            var inventory = new Petstore.Types.Services.Store.GetInventory.Response200()
            {
                { "2321", 10 },
                { "2322", 5 }
            };

            var users = new Petstore.Types.User[]
            {
                new Petstore.Types.User()
                {
                    Id = 1,
                    Username = "aarrgard",
                    Email = "a@b.c",
                    Password = "pwd1"
                },
                new Petstore.Types.User()
                {
                    Id = 2,
                    Username = "barrgard",
                    Email = "b@b.c",
                    Password = "pwd2"
                }
            };

            var petMoq = new Mock<Petstore.Services.IPet>(MockBehavior.Strict);
            var storeMoq = new Mock<Petstore.Services.IStore>(MockBehavior.Strict);
            var userMoq = new Mock<Petstore.Services.IUser>(MockBehavior.Strict);


            ctx.AddServerAndClientService(petMoq.Object, config);
            ctx.AddServerAndClientService(storeMoq.Object, config);
            ctx.AddServerAndClientService(userMoq.Object, config);

            await ctx.StartAsync();

            var petProxy = ctx.ClientServiceProvider.GetRequiredService<Petstore.Services.IPet>();
            var storeProxy = ctx.ClientServiceProvider.GetRequiredService<Petstore.Services.IStore>();
            var userProxy = ctx.ClientServiceProvider.GetRequiredService<Petstore.Services.IUser>();
            //
            // Pet
            //
            petMoq.Setup(o => o.AddPet(It.Is<Petstore.Types.Pet>(a => CompareStructs(pet, a)), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await petProxy.AddPet(pet);
            Assert.AreEqual(1, petMoq.Invocations.Count);

            petMoq.Setup(o => o.DeletePet(It.Is<long>(a => a == pet.Id), It.Is<string>(a => a == api_key), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await petProxy.DeletePet(pet.Id, api_key);
            Assert.AreEqual(2, petMoq.Invocations.Count);

            petMoq.Setup(o => o.FindPetsByStatus(It.Is<IEnumerable<string>>(a => CompareStructs(a, statuses)), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new[] { pet }.AsEnumerable()));
            CompareStructs(typeof(IEnumerable<Petstore.Services.IPet>), new[] { pet }, await petProxy.FindPetsByStatus(statuses));
            Assert.AreEqual(3, petMoq.Invocations.Count);

            petMoq.Setup(o => o.FindPetsByTags(It.Is<IEnumerable<string>>(a => CompareStructs(a, tags.Select(t => t.Name))), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new[] { pet }.AsEnumerable()));
            CompareStructs(typeof(IEnumerable<Petstore.Services.IPet>), new[] { pet }, await petProxy.FindPetsByTags(tags.Select(t => t.Name)));
            Assert.AreEqual(4, petMoq.Invocations.Count);

            petMoq.Setup(o => o.GetPetById(It.Is<long>(a => a == pet.Id), It.IsAny<CancellationToken>())).Returns(Task.FromResult(pet));
            CompareStructs(pet, await petProxy.GetPetById(pet.Id));
            Assert.AreEqual(5, petMoq.Invocations.Count);

            petMoq.Setup(o => o.UpdatePet(It.Is<Petstore.Types.Pet>(a => CompareStructs(a,pet)), It.IsAny<CancellationToken>())).Returns(Task.FromResult(pet));
            await petProxy.UpdatePet(pet);
            Assert.AreEqual(6, petMoq.Invocations.Count);

            petMoq.Setup(o => o.UpdatePetWithForm(It.Is<long>(a => a == pet.Id), It.Is<string>(a => a == pet.Name), It.Is<string>(a => a == pet.Status), It.IsAny<CancellationToken>())).Returns(Task.FromResult(pet));
            await petProxy.UpdatePetWithForm(pet.Id, pet.Name, pet.Status);
            Assert.AreEqual(7, petMoq.Invocations.Count);

            var streamContent = new byte[] { 1, 2, 3 };
            petMoq.Setup(o => o.UploadFile(It.Is<long>(a => a == pet.Id), It.Is<string>(a => a == "Additional data"), It.Is<Stream>(a => CompareStructs(a, new MemoryStream(streamContent))), It.IsAny<CancellationToken>())).Returns(Task.FromResult(apiResponse));
            CompareStructs(apiResponse, await petProxy.UploadFile(pet.Id, "Additional data", new MemoryStream(streamContent)));
            Assert.AreEqual(8, petMoq.Invocations.Count);


            //
            // Store
            //
            storeMoq.Setup(o => o.PlaceOrder(It.Is<Petstore.Types.Order>(a => CompareStructs(a, order)), It.IsAny<CancellationToken>())).Returns(Task.FromResult(order));
            CompareStructs(order, await storeProxy.PlaceOrder(order));
            Assert.AreEqual(1, storeMoq.Invocations.Count);

            storeMoq.Setup(o => o.DeleteOrder(It.Is<long>(a => a == order.Id), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await storeProxy.DeleteOrder(order.Id);
            Assert.AreEqual(2, storeMoq.Invocations.Count);

            storeMoq.Setup(o => o.GetOrderById(It.Is<long>(a => a == order.Id), It.IsAny<CancellationToken>())).Returns(Task.FromResult(order));
            CompareStructs(order, await storeProxy.GetOrderById(order.Id));
            Assert.AreEqual(3, storeMoq.Invocations.Count);

            storeMoq.Setup(o => o.GetInventory(It.IsAny<CancellationToken>())).Returns(Task.FromResult(inventory));
            var inventory2 = await storeProxy.GetInventory();
            //Assert.AreNotSame(inventory, inventory2);
            CompareStructs(inventory, inventory2);
            Assert.AreEqual(4, storeMoq.Invocations.Count);

            //
            // User
            //
            userMoq.Setup(o => o.CreateUser(It.Is<Petstore.Types.User>(a => CompareStructs(a,users[0])), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await userProxy.CreateUser(users[0]);
            Assert.AreEqual(1, userMoq.Invocations.Count);

            userMoq.Setup(o => o.CreateUsersWithArrayInput(It.Is<IEnumerable<Petstore.Types.User>>(a => CompareStructs(a.ToArray(), users)), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await userProxy.CreateUsersWithArrayInput(users);
            Assert.AreEqual(2, userMoq.Invocations.Count);

            userMoq.Setup(o => o.CreateUsersWithListInput(It.Is<IEnumerable<Petstore.Types.User>>(a => CompareStructs(a.ToArray(), users)), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await userProxy.CreateUsersWithListInput(users);
            Assert.AreEqual(3, userMoq.Invocations.Count);

            userMoq.Setup(o => o.DeleteUser(It.Is<string>(a => a == users[0].Username), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await userProxy.DeleteUser(users[0].Username);
            Assert.AreEqual(4, userMoq.Invocations.Count);

            userMoq.Setup(o => o.GetUserByName(It.Is<string>(a => a == users[0].Username), It.IsAny<CancellationToken>())).Returns(Task.FromResult(users[0]));
            CompareStructs(users[0], await userProxy.GetUserByName(users[0].Username));
            Assert.AreEqual(5, userMoq.Invocations.Count);

            userMoq.Setup(o => o.LoginUser(It.Is<string>(a => a == users[0].Username), It.Is<string>(a => a == users[0].Password), It.IsAny<CancellationToken>())).Returns(Task.FromResult(api_key));
            CompareStructs(api_key, await userProxy.LoginUser(users[0].Username, users[0].Password));
            Assert.AreEqual(6, userMoq.Invocations.Count);

            userMoq.Setup(o => o.LogoutUser(It.IsAny<CancellationToken>())).Returns(Task.FromResult(api_key));
            await userProxy.LogoutUser();
            Assert.AreEqual(7, userMoq.Invocations.Count);


            // Exception
            //
            try
            {
                userMoq.Setup(o => o.UpdateUser(It.Is<string>(a => a == "kalle"), It.Is<Petstore.Types.User>(a => CompareStructs(a, users[0])), It.IsAny<CancellationToken>())).Throws(new UserNotFoundException());
                await userProxy.UpdateUser("kalle", users[0]);
                Assert.Fail();
            }
            catch (UserNotFoundException)
            {
                // ok
            }
            Assert.AreEqual(8, userMoq.Invocations.Count);
        }
    }
}
