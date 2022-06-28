using ei8.Cortex.Subscriptions.Application.Interface.Service;

namespace ei8.Cortex.Subscriptions.In.Api.Settings
{
    public class EnvironmentSettingsService : ISettingsService
    {
        public EnvironmentSettingsService(IConfiguration config)
        {
            SubscriptionsDatabasePath = config.GetSection(EnvironmentVariableKeys.SubscriptionsDatabasePath).Value;
        }

        public string SubscriptionsDatabasePath { get; set; }
    }
}
