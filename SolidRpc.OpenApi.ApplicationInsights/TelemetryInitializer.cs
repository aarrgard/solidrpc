using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using System;

namespace SolidRpc.OpenApi.ApplicationInsights
{
    public class TelemetryInitializer : ITelemetryInitializer
    {
        public TelemetryInitializer(IConfiguration configuration)
        {
            if (configuration == null) return;
            RoleName = configuration["WEBSITE_HOSTNAME"];
            RoleInstance = Environment.MachineName;
            SetCloudData = !string.IsNullOrEmpty(RoleName) && !string.IsNullOrEmpty(RoleInstance);
        }

        private bool SetCloudData { get; set; }
        private string RoleName { get; set; }
        private string RoleInstance { get; set; }

        public void Initialize(ITelemetry telemetry)
        {
            if(SetCloudData)
            {
                telemetry.Context.Cloud.RoleName = RoleName;
                telemetry.Context.Cloud.RoleInstance = RoleInstance;
            }
        }
    }
}
