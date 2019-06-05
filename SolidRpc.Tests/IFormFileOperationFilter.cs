using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
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
            StreamSchema = new Schema()
            {
                Type = "string",
                Format = "binary"
            };
        }
        /// <summary>
        /// 
        /// </summary>
        public Schema StreamSchema { get; }

        /// <summary>
        /// 
        /// </summary>
        public void Apply(Operation operation, OperationFilterContext context)
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
                .ToList().ForEach(o => {
                    OperationParameterIsStream(operation, context, o);
            });
        }

        private void OperationParameterIsStream(Operation operation, OperationFilterContext context, ParameterInfo o)
        {
            context.SchemaRegistry.Definitions.Remove("IFormFile");
            operation.Parameters.Clear();
            operation.Parameters.Add(new Swashbuckle.AspNetCore.Swagger.BodyParameter()
            {
                Name = o.Name,
                Schema = StreamSchema
            });
        }

        private void OperationReturnsStream(Operation operation, OperationFilterContext context)
        {
            context.SchemaRegistry.Definitions.Remove("IFormFile");
            operation.Responses.Clear();
            operation.Responses.Add("200", new Response() { Schema = StreamSchema });
        }
    }
}