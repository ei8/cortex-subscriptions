using ei8.Cortex.Subscriptions.Application.Interface.Service;

namespace ei8.Cortex.Subscriptions.In.Api.Settings
{
    public class EnvironmentSettingsService : ISettingsService
    {
        public EnvironmentSettingsService(IConfiguration config)
        {
            SubscriptionsDatabasePath = config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsDatabasePath);
            SubscriptionsPollingIntervalSeconds = config.GetValue<int>(EnvironmentVariableKeys.SubscriptionsPollingIntervalSeconds);
        }

        public string SubscriptionsDatabasePath { get; set; }

        public int SubscriptionsPollingIntervalSeconds { get; set; }
    }
}
