using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a protected resource
    /// </summary>
    public class ProtectedResource
    {
        /// <summary>
        /// The source of the protected resource.
        /// Add /SolidRpc/Abstractions/Services/.well-known/openid-configuration to get the keys
        /// Add /SolidRpc/Abstractions/Services/ISolidRpcContentHandler/GetProtectedContentAsync/==data==
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The expiration date
        /// </summary>
        public DateTimeOffset Expiration { get; set; }

        /// <summary>
        /// The resource
        /// </summary>
        public string Resource { get; set; }
    }

    /// <summary>
    /// Provides extension methods for a protected resource
    /// </summary>
    public static class ProtectedResourceExtensions
    {
        /// <summary>
        /// Serializes the protected resource into a byte array
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public static async Task<byte[]> ToByteArrayAsync(this ProtectedResource pr, Func<string, byte[], Task<byte[]>> sign)
        {
            var ms = new MemoryStream();
            ms.WriteByte(3);
            var expTime = BitConverter.GetBytes(pr.Expiration.ToUnixTimeSeconds());
            if (expTime.Length != 8) throw new Exception();
            ms.Write(expTime, 0, expTime.Length);
            WriteLenAndBytes(ms, Encoding.UTF8.GetBytes(pr.Source));
            WriteLenAndBytes(ms, Encoding.UTF8.GetBytes(pr.Resource));
            
            var signature = await sign(pr.Source, ms.ToArray());
            WriteLenAndBytes(ms, signature);

            return ms.ToArray();
        }

        /// <summary>
        /// Serializes the protected resource into a byte array
        /// </summary>
        /// <param name="protectedResource"></param>
        /// <returns></returns>
        public static async Task<ProtectedResource> AsProtectedResourceAsync(this byte[] protectedResource, Func<string, byte[], byte[], Task<bool>> verify)
        {
            var ms = new MemoryStream(protectedResource);
            var version = ms.ReadByte();
            if (version != 3) throw new ArgumentException();

            var pr = new ProtectedResource();
            pr.Expiration = DateTimeOffset.FromUnixTimeSeconds(BitConverter.ToInt64(ReadBytes(ms, 8),0));
            pr.Source = Encoding.UTF8.GetString(ReadLenAndBytes(ms));
            pr.Resource = Encoding.UTF8.GetString(ReadLenAndBytes(ms));

            var dataLen = (int)ms.Position;
            var signature = ReadLenAndBytes(ms);

            Array.Resize(ref protectedResource, dataLen);

            if(await verify(pr.Source, protectedResource, signature))
            {
                return pr;
            }
            throw new Exception("Cannot verify signature");
        }

        private static byte[] ReadLenAndBytes(MemoryStream ms)
        {
            var len = BitConverter.ToUInt16(ReadBytes(ms, 2), 0);
            return ReadBytes(ms, len);
        }

        private static byte[] ReadBytes(MemoryStream ms, int len)
        {
            var b = new byte[len];
            ms.Read(b, 0, b.Length);
            return b;
        }

        private static void WriteLenAndBytes(MemoryStream ms, byte[] vs)
        {
            var len = BitConverter.GetBytes((ushort)vs.Length);
            ms.Write(len, 0, len.Length);
            ms.Write(vs, 0, vs.Length);
        }
    }
}
