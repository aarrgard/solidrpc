using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class IFormFileOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public IFormFileOperationFilter()
        {
            StreamSchema = new OpenApiSchema()
            {
                Type = "string",
                Format = "binary"
            };
        }
        ///// <summary>
        ///// 
        ///// </summary>
        public OpenApiSchema StreamSchema { get; }


        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (typeof(FileStreamResult).IsAssignableFrom(context.MethodInfo.ReturnType) ||
                typeof(Task<FileStreamResult>).IsAssignableFrom(context.MethodInfo.ReturnType))
            {
                OperationReturnsStream(operation, context);
            }
            if (typeof(IFormFile).IsAssignableFrom(context.MethodInfo.ReturnType) ||
                typeof(Task<IFormFile>).IsAssignableFrom(context.MethodInfo.ReturnType))
            {
                OperationReturnsStream(operation, context);
            }
            context.MethodInfo.GetParameters()
                .Where(o => typeof(IFormFile).IsAssignableFrom(o.ParameterType))
                .ToList().ForEach(o =>
                {
                    OperationParameterIsStream(operation, context, o);
                });
        }

        private void OperationParameterIsStream(OpenApiOperation operation, OperationFilterContext context, ParameterInfo pi)
        {
            var param = operation.Parameters.FirstOrDefault(o => o.Name == pi.Name);
            if(param != null)
            {
                param.Schema = StreamSchema;
            }
        }

        private void OperationReturnsStream(OpenApiOperation operation, OperationFilterContext context)
        {
            //context.SchemaRegistry.Definitions.Remove("IFormFile");
            operation.Responses.Clear();
            var resp = new OpenApiResponse();
            resp.Content.Add("application/json", new OpenApiMediaType()
            {
                Schema = StreamSchema
            });
            operation.Responses.Add("200", resp);
        }
    }
}