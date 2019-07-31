using Microsoft.Extensions.DependencyInjection;
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
    public class CodeGenTest : SwaggerTestBase
    {
        /// <summary>
        /// Returns the spec folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected override DirectoryInfo GetSpecFolder(string folderName)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "CodeGen");
            path = Path.Combine(path, folderName);
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Tests generating code from a swagger file
        /// </summary>
        [Test]
        public async Task TestCodeGenLocal()
        {
            string swaggerSpec;
            using (var ctx = await StartKestrelHostContextAsync())
            {
                var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                swaggerSpec = await AssertOk(resp);
           }

            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "CodeGen");
            path = Path.Combine(path, "Local");
            path = Path.Combine(path, "swagger.json");
            var fi = new FileInfo(path);
            using (var tw = fi.CreateText())
            {
                await tw.WriteAsync(swaggerSpec);
            }           
        }

        /// <summary>
        /// Tests generating code from a swagger file
        /// </summary>
        [Test]
        public void TestCodeGen()
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "CodeGen");
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            foreach(var subDir in dir.GetDirectories())
            {
                CreateCode(subDir, true);
            }
        }

        private void CreateCode(DirectoryInfo dir, bool onlyCompare)
        {
            var openApiFiles = dir.GetFiles("*.json");
            if(openApiFiles.Length != 1)
            {
                Assert.Fail("Cannot find json file in folder:" + dir.FullName);
            }
            try
            {
                Program.MainWithExeptions(new[] {
                "-openapi2code",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.CodeGen.{dir.Name}",
                openApiFiles[0].Name }).Wait();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

            // await proxy.AddPet(pet);
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.AddPet(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(pet, args[0]);
                    Assert.AreEqual(CancellationToken.None, args[1]);
                    return Task.CompletedTask;
                });

            // await proxy.DeletePet(api_key, pet.Id);
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.DeletePet(null, 0, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(3, args.Length);
                    CompareStructs(api_key, args[0]);
                    CompareStructs(pet.Id, args[1]);
                    CompareStructs(CancellationToken.None, args[2]);
                    return Task.CompletedTask;
                });

            //  await proxy.FindPetsByStatus(statuses);
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.FindPetsByStatus(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(statuses, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.FromResult<IEnumerable<Petstore.Types.Pet>>(new Petstore.Types.Pet[] { pet });
                });

            //  await proxy.FindPetsByTags(statuses);
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.FindPetsByTags(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(typeof(IEnumerable<string>), tags.Select(o => o.Name), args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.FromResult<IEnumerable<Petstore.Types.Pet>>(new Petstore.Types.Pet[] { pet });
                });

            //  await proxy.GetPetById(pet.Id)
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.GetPetById(0, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(pet.Id, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.FromResult(pet);
                });

            //  await proxy.UpdatePet(pet)
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.UpdatePet(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(pet, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.FromResult(pet);
                });

            //  await proxy.UpdatePet(pet)
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.UpdatePetWithForm(0, null, null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(4, args.Length);
                    CompareStructs(pet.Id, args[0]);
                    CompareStructs(pet.Name, args[1]);
                    CompareStructs(pet.Status, args[2]);
                    CompareStructs(CancellationToken.None, args[3]);
                    return Task.FromResult(pet);
                });


            //  await proxy.UpdatePet(pet)
            ctx.CreateServerInterceptor<Petstore.Services.IPet>(
                o => o.UploadFile(0, null, null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(4, args.Length);
                    CompareStructs(pet.Id, args[0]);
                    CompareStructs("Additional data", args[1]);
                    CompareStructs(new MemoryStream(new byte[] { 1, 2, 3 }), args[2]);
                    CompareStructs(CancellationToken.None, args[3]);
                    return Task.FromResult(apiResponse);
                });


            //  await proxy.PlaceOrder(pet)
            ctx.CreateServerInterceptor<Petstore.Services.IStore>(
                o => o.PlaceOrder(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(order, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.FromResult((Petstore.Types.Order)args[0]);
                });

            //  await proxy.DeleteOrder(124)
            ctx.CreateServerInterceptor<Petstore.Services.IStore>(
                o => o.DeleteOrder(0, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(order.Id, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.CompletedTask;
                });

            //  await proxy.DeleteOrder(124)
            ctx.CreateServerInterceptor<Petstore.Services.IStore>(
                o => o.GetOrderById(0, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(order.Id, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.FromResult(order);
                });

            //  await proxy.DeleteOrder(124)
            ctx.CreateServerInterceptor<Petstore.Services.IStore>(
                o => o.GetInventory(CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(1, args.Length);
                    CompareStructs(CancellationToken.None, args[0]);
                    return Task.FromResult(inventory);
                });

            //  await proxy.CreateUser(null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.CreateUser(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(users[0], args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.CompletedTask;
                });

            //  await proxy.CreateUser(null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.CreateUsersWithArrayInput(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(typeof(IEnumerable<Petstore.Types.User>), users, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.CompletedTask;
                });

            //  await proxy.CreateUser(null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.CreateUsersWithListInput(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(typeof(IEnumerable<Petstore.Types.User>), users, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.CompletedTask;
                });


            //  await proxy.DeleteUser(null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.DeleteUser(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(users[0].Username, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.CompletedTask;
                });

            //  await proxy.DeleteUser(null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.DeleteUser(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(users[0].Username, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.CompletedTask;
                });

            //  await proxy.GetUserByName(null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.GetUserByName(null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(2, args.Length);
                    CompareStructs(users[0].Username, args[0]);
                    CompareStructs(CancellationToken.None, args[1]);
                    return Task.FromResult(users[0]);
                });


            //  await proxy.LoginUser(null,null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.LoginUser(null, null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(3, args.Length);
                    CompareStructs(users[0].Username, args[0]);
                    CompareStructs(users[0].Password, args[1]);
                    CompareStructs(CancellationToken.None, args[2]);
                    return Task.FromResult(api_key);
                });

            //  await proxy.LoginUser(null,null)
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.LogoutUser(CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(1, args.Length);
                    CompareStructs(CancellationToken.None, args[0]);
                    return Task.CompletedTask;
                });

            //  Exception - await UpdateUser
            ctx.CreateServerInterceptor<Petstore.Services.IUser>(
                o => o.UpdateUser(null, null, CancellationToken.None),
                config,
                args =>
                {
                    Assert.AreEqual(3, args.Length);
                    CompareStructs("kalle", args[0]);
                    CompareStructs(users[0], args[1]);
                    CompareStructs(CancellationToken.None, args[2]);
                    throw new UserNotFoundException();
                });

            ctx.AddOpenApiProxy<Petstore.Services.IPet>(config);
            ctx.AddOpenApiProxy<Petstore.Services.IStore>(config);
            ctx.AddOpenApiProxy<Petstore.Services.IUser>(config);

            await ctx.StartAsync();

            var petProxy = ctx.ClientServiceProvider.GetRequiredService<Petstore.Services.IPet>();
            var storeProxy = ctx.ClientServiceProvider.GetRequiredService<Petstore.Services.IStore>();
            var userProxy = ctx.ClientServiceProvider.GetRequiredService<Petstore.Services.IUser>();
            //
            // Pet
            //
            await petProxy.AddPet(pet);
            await petProxy.DeletePet(api_key, pet.Id);
            CompareStructs(typeof(IEnumerable<Petstore.Services.IPet>), new[] { pet }, await petProxy.FindPetsByStatus(statuses));
            CompareStructs(typeof(IEnumerable<Petstore.Services.IPet>), new[] { pet }, await petProxy.FindPetsByTags(tags.Select(o => o.Name)));
            CompareStructs(pet, await petProxy.GetPetById(pet.Id));
            await petProxy.UpdatePet(pet);
            await petProxy.UpdatePetWithForm(pet.Id, pet.Name, pet.Status);
            CompareStructs(apiResponse, await petProxy.UploadFile(pet.Id, "Additional data", new MemoryStream(new byte[] { 1, 2, 3 })));


            //
            // Store
            //
            CompareStructs(order, await storeProxy.PlaceOrder(order));
            await storeProxy.DeleteOrder(order.Id);
            CompareStructs(order, await storeProxy.GetOrderById(order.Id));
            var inventory2 = await storeProxy.GetInventory();
            Assert.AreNotSame(inventory, inventory2);
            CompareStructs(inventory, inventory2);

            //
            // User
            //
            await userProxy.CreateUser(users[0]);
            await userProxy.CreateUsersWithArrayInput(users);
            await userProxy.CreateUsersWithListInput(users);
            await userProxy.DeleteUser(users[0].Username);
            CompareStructs(users[0], await userProxy.GetUserByName(users[0].Username));
            CompareStructs(api_key, await userProxy.LoginUser(users[0].Username, users[0].Password));
            await userProxy.LogoutUser();

            //
            // Exception
            //
            try
            {
                await userProxy.UpdateUser("kalle", users[0]);
                Assert.Fail();
            }
            catch (UserNotFoundException)
            {
                // ok
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestUrlParam()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestUrlParam).Substring(4));

                // await proxy.DeletePet(api_key, pet.Id);
                ctx.CreateServerInterceptor<UrlParam.Services.IUrlParam>(
                    o => o.ProxyIntegerInPath(1, CancellationToken.None),
                    config,
                    args =>
                    {
                        CompareStructs(3, args[0]);
                        CompareStructs(CancellationToken.None, args[1]);
                        return Task.FromResult((int)args[0]);
                    });


                ctx.AddOpenApiProxy<UrlParam.Services.IUrlParam>(config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<UrlParam.Services.IUrlParam>();

                await proxy.ProxyIntegerInPath(3);
            }
        }
    }
}
