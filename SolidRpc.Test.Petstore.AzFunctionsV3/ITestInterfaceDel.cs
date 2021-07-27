using SolidRpc.Abstractions.Types;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.AzFunctionsV3
{
    public interface ITestInterfaceDel
    {
        Task<string> RunNodeService(CancellationToken cancellationToke = default);
        Task<string> MyFunc(string arg1);
        Task<string> MyFunc(string arg1, string arg2);
        Task<FileContent> UploadFile(string container, FileContent fileContent);
    }

    public class TestDelImplementation : ITestInterfaceDel
    {
        public TestDelImplementation(ITestInterface testInterface)
        {
            TestInterface = testInterface;
        }

        public ITestInterface TestInterface { get; }

        public Task<string> MyFunc(string arg1)
        {
            return TestInterface.MyFunc(arg1);
        }

        public Task<string> MyFunc(string arg1, string arg2)
        {
            return TestInterface.MyFunc(arg1, arg2);
        }

        public Task<string> RunNodeService(CancellationToken cancellationToke = default)
        {
            return TestInterface.RunNodeService(cancellationToke);
        }

        public Task<FileContent> UploadFile(string container, FileContent fileContent)
        {
            return TestInterface.UploadFile(container, fileContent);
        }
    }
}
