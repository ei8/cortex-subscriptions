using ei8.Cortex.Subscriptions.Application;
using ei8.Net.Http.Notifications;
using Microsoft.Extensions.Configuration;
using System;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Process.Services
{
    public class EnvironmentSettingsService : ISettingsService
    {
        public EnvironmentSettingsService(IConfiguration config)
        {
            this.DatabasePath = config.GetSection(EnvironmentVariableKeys.SubscriptionsDatabasePath).Value;
            this.PollingIntervalSeconds = Convert.ToInt32(config.GetSection(EnvironmentVariableKeys.SubscriptionsPollingIntervalSeconds).Value);

            this.PushSettings = new PushNotificationSettings(
                pushOwner: config.GetSection(EnvironmentVariableKeys.SubscriptionsPushOwner).Value,
                pushPublicKey: config.GetSection(EnvironmentVariableKeys.SubscriptionsPushPublicKey).Value,
                pushPrivateKey: config.GetSection(EnvironmentVariableKeys.SubscriptionsPushPrivateKey).Value
            );
        }

        public string DatabasePath { get; set; }
        public int PollingIntervalSeconds { get; set; }
        public PushNotificationSettings PushSettings { get; set; }
    }
}
