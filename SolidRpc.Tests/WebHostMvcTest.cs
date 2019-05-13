using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests
{
    public class WebHostMvcTest : WebHostTest
    {
        public override void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().ConfigureApplicationPartManager(apm =>
            {
                var ap = new AssemblyPart(GetType().Assembly);
                apm.ApplicationParts.Add(ap);
            });

            return services.BuildServiceProvider();
        }
    }
}