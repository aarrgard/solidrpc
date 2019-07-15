using System;

namespace SolidRpc.OpenApi.Binder.Http.Multipart
{
    public class HeaderUtilities
    {
        public static string RemoveQuotes(string s)
        {
            if (s.StartsWith("\"") && s.EndsWith("\""))
            {
                return s.Substring(1, s.Length - 2);
            }
            return s;
        }
    }
}