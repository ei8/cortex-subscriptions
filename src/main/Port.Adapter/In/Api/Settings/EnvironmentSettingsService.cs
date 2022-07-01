using ei8.Cortex.Subscriptions.Domain.Model;

namespace ei8.Cortex.Subscriptions.In.Api.Settings
{
    public class EnvironmentSettingsService : ISettingsService
    {
        public EnvironmentSettingsService(IConfiguration config)
        {
            DatabasePath = config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsDatabasePath);
            PollingIntervalSeconds = config.GetValue<int>(EnvironmentVariableKeys.SubscriptionsPollingIntervalSeconds);
            PushOwner = config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsPushOwner);
            PushPublicKey = config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsPushPublicKey);
            PushPrivateKey = config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsPushPrivateKey);
        }

        public string DatabasePath { get; set; }
        public int PollingIntervalSeconds { get; set; }
        public string PushOwner { get; set; }
        public string PushPublicKey { get; set; }
        public string PushPrivateKey { get; set; }
    }
}
