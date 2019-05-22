using SolidRpc.Swagger.Model.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.Swagger.Binder.V2
{
    public class MethodInfoV2 : IMethodInfo
    {
        public static ParameterObject GetParameterObject(OperationObject operationObject, ParameterInfo parameterInfo)
        {
            var parameters = operationObject.Parameters
                .Where(o => o.Name == parameterInfo.Name);
            var parameter = parameters.FirstOrDefault();
            if(parameter == null)
            {
                throw new Exception($"Cannot find parameter {parameterInfo.Name} among parameters({string.Join(",",parameters.Select(o => o.Name))}).");
            }
            return parameter;
        }

        public MethodInfoV2(OperationObject operationObject, MethodInfo methodInfo)
        {
            OperationObject = operationObject ?? throw new ArgumentNullException(nameof(operationObject));
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            Arguments = CreateArguments();
        }

        private IMethodArgument[] CreateArguments()
        {
            return MethodInfo.GetParameters().Select(parameterInfo => {
                var parameterObject = GetParameterObject(OperationObject, parameterInfo);
                return new MethodArgument(parameterObject, parameterInfo);
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
