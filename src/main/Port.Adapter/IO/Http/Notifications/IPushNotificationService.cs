using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.IO.Http.Notifications
{
    public interface IPushNotificationService
    {
        Task SendAsync(PushNotificationPayload payload, WebPushReceiver subscription);
    }
}
