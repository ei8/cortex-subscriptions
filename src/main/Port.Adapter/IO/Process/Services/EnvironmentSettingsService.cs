using ei8.Cortex.Subscriptions.Application;
using ei8.Net.Email.Smtp.Notifications;
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

            this.PushSettings = new WebPushNotificationSettings(
                pushOwner: config.GetSection(EnvironmentVariableKeys.SubscriptionsPushOwner).Value,
                pushPublicKey: config.GetSection(EnvironmentVariableKeys.SubscriptionsPushPublicKey).Value,
                pushPrivateKey: config.GetSection(EnvironmentVariableKeys.SubscriptionsPushPrivateKey).Value
            );

            this.SmtpSettings = new SmtpNotificationSettings()
            {
                ServerAddress = config.GetSection(EnvironmentVariableKeys.SubscriptionsSmtpServerAddressKey).Value,
                Port = int.TryParse(config.GetSection(EnvironmentVariableKeys.SubscriptionsSmtpPortKey).Value, out int port) ? port : 0,
                UseSsl = bool.TryParse(config.GetSection(EnvironmentVariableKeys.SubscriptionsSmtpUseSslKey).Value, out bool useSsl) ? useSsl : false,
                SenderName = config.GetSection(EnvironmentVariableKeys.SubscriptionsSmtpSenderNameKey).Value,
                SenderAddress = config.GetSection(EnvironmentVariableKeys.SubscriptionsSmtpSenderAddressKey).Value,
                SenderUsername = config.GetSection(EnvironmentVariableKeys.SubscriptionsSmtpSenderUsernameKey).Value,
                SenderPassword = config.GetSection(EnvironmentVariableKeys.SubscriptionsSmtpSenderPasswordKey).Value,
            };

            this.CortexGraphOutBaseUrl = config.GetSection(EnvironmentVariableKeys.SubscriptionsCortexGraphOutBaseUrl).Value;
        }

        public string DatabasePath { get; set; }
        public int PollingIntervalSeconds { get; set; }
        public WebPushNotificationSettings PushSettings { get; set; }
        public SmtpNotificationSettings SmtpSettings { get; set; }
        public string CortexGraphOutBaseUrl { get; private set; }
    }
}
