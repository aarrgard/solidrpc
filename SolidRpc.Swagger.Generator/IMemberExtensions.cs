using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public static class IMemberExtensions
    {
        public static IEnumerable<T> GetChildren<T>(this IEnumerable<IMember> members)
        {
            return members.SelectMany(o =>
            {
                if (o is T hit)
                {
                    return new T[] { hit };
                }
                else
                {
                    return o.Members.GetChildren<T>();
                }
            });
        }
    }
}
