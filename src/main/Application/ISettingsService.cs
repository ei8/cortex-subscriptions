using ei8.Net.Email.Smtp.Notifications;
using ei8.Net.Http.Notifications;

namespace ei8.Cortex.Subscriptions.Application
{
    public interface ISettingsService
    {
        string DatabasePath { get; set; }
        int PollingIntervalSeconds { get; set; }
        WebPushNotificationSettings PushSettings { get; set; }
        SmtpNotificationSettings SmtpSettings { get; set; }
        string CortexGraphOutBaseUrl { get; }
    }
}
