using ei8.Net.Http.Notifications;

namespace ei8.Cortex.Subscriptions.Application
{
    public interface ISettingsService
    {
        string DatabasePath { get; set; }
        int PollingIntervalSeconds { get; set; }

        PushNotificationSettings PushSettings { get; set; }
    }
}
