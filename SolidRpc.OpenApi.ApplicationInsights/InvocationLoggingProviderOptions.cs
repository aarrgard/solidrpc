namespace SolidRpc.OpenApi.ApplicationInsights
{
    public class InvocationLoggingProviderOptions
    {
        public InvocationLoggingProviderOptions(LogSettings logSettings, string propertyActivator)
        {
            LogSettings = logSettings;
            PropertyActivator = propertyActivator;
        }
        public LogSettings LogSettings { get;  }
        public string PropertyActivator { get; }
    }
}