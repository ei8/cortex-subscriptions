using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Net.Http.Notifications;

namespace ei8.Cortex.Subscriptions.In.Api.Settings
{
    public class EnvironmentSettingsService : ISettingsService
    {
        public EnvironmentSettingsService(IConfiguration config)
        {
            this.DatabasePath = config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsDatabasePath);
            this.PollingIntervalSeconds = config.GetValue<int>(EnvironmentVariableKeys.SubscriptionsPollingIntervalSeconds);

            this.PushSettings = new PushNotificationSettings(
                pushOwner: config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsPushOwner),
                pushPublicKey: config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsPushPublicKey),
                pushPrivateKey: config.GetValue<string>(EnvironmentVariableKeys.SubscriptionsPushPrivateKey)
            );
        }

        public string DatabasePath { get; set; }
        public int PollingIntervalSeconds { get; set; }
        public PushNotificationSettings PushSettings { get; set; }
    }
}
