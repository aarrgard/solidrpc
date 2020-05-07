using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.Test.Petstore.AzFunctionsV3
{
    public interface ITestInterface 
    {
        Task<string> MyFunc(string arg1);
        Task<string> MyFunc(string arg1, string arg2);
        Task<FileContent> UploadFile(string container, FileContent fileContent);
    }

    public class TestImplementation : ITestInterface

    {
        public Task<string> MyFunc(string arg)
        {
            throw new NotImplementedException();
        }

        public Task<string> MyFunc(string arg1, string arg2)
        {
            throw new NotImplementedException();
        }

        public async Task<FileContent> UploadFile(string container, FileContent fileContent)
        {
            string strContent;
            using (var sr = new StreamReader(fileContent.Content))
            {
                strContent = sr.ReadToEnd();
            }
            fileContent.ETag = $"{Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 })}";
            fileContent.Content = new MemoryStream(Encoding.UTF8.GetBytes(strContent));
            fileContent.CharSet = Encoding.UTF8.HeaderName;
            return fileContent;
        }
    }
}
