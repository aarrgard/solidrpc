using Microsoft.Azure.WebJobs.Description;
using System;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
    }
}
