using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.AzFunctionsV3
{
    public interface ITestInterface 
    {
        Task<string> MyFunc(string arg);
        Task<FileContent> UploadFile(string container, FileContent fileContent);
    }

    public class TestImplementation : ITestInterface

    {
        public Task<string> MyFunc(string arg)
        {
            throw new NotImplementedException();
        }

        public async Task<FileContent> UploadFile(string container, FileContent fileContent)
        {
            return fileContent;
        }
    }
}
