using SolidRpc.Swagger.Binder;
using System;

namespace SolidRpc.Swagger.Model
{
    public static class ISwaggerSpecExtensions
    {
        /// <summary>
        /// Returns the method binder for supplied spec.
        /// </summary>
        /// <param name="swaggerSpec"></param>
        /// <returns></returns>
        public static IMethodBinder GetMethodBinder(this ISwaggerSpec swaggerSpec)
        {
            if(swaggerSpec is V2.SwaggerObject v2)
            {
                return new Binder.V2.MethodBinderV2(v2);
            }
            throw new NotImplementedException($"Cannot get binder for {swaggerSpec.GetType().FullName}");
        }
    }
}
