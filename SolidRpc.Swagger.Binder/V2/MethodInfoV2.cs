using SolidRpc.Swagger.Model.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.Swagger.Binder.V2
{
    public class MethodInfoV2 : IMethodInfo
    {
        public static ParameterObject GetMethodArgument(OperationObject operationObject, ParameterInfo parameterInfo)
        {
            var parameters = operationObject.Parameters.Where(o => o.Name == parameterInfo.Name);

            return parameters.FirstOrDefault();
        }

        public MethodInfoV2(OperationObject operationObject, MethodInfo methodInfo)
        {
            OperationObject = operationObject;
            MethodInfo = methodInfo;
            Arguments = CreateArguments();
        }

        private IMethodArgument[] CreateArguments()
        {
            return MethodInfo.GetParameters().Select(o => {
                var parameterObject = GetMethodArgument(OperationObject, o);
                return new MethodArgument(parameterObject, o);
            }).ToArray();
        }

        public OperationObject OperationObject { get; }

        public MethodInfo MethodInfo { get; }

        public IMethodArgument[] Arguments { get; }
        IEnumerable<IMethodArgument> IMethodInfo.Arguments => Arguments;

        public string OperationId => OperationObject.OperationId;

        public void BindArguments(IHttpRequest request, object[] args)
        {
            for(int i = 0; i < Arguments.Length; i++)
            {
                Arguments[i].BindArgument(request, args[i]);
            }
        }

    }
}
