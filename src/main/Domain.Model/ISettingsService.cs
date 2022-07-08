using ei8.Net.Http.Notifications;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface ISettingsService
    {
        string DatabasePath { get; set; }
        int PollingIntervalSeconds { get; set; }

        PushNotificationSettings PushSettings { get; set; }
    }
}
