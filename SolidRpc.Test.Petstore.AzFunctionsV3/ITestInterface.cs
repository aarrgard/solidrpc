using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.AzFunctionsV3
{
    public interface ITestInterface 
    {
        Task<FileContent> UploadFile(string container, FileContent fileContent);
    }

    public class TestImplementation : ITestInterface

    {
        public async Task<FileContent> UploadFile(string container, FileContent fileContent)
        {
            return fileContent;
        }
    }
}
