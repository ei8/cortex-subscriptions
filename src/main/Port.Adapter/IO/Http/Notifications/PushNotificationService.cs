using ei8.Cortex.Subscriptions.Domain.Model;
using System.Text.Json;
using System.Threading.Tasks;
using WebPush;

namespace ei8.Cortex.Subscriptions.IO.Http.Notifications
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly VapidDetails vapidDetails;

        public PushNotificationService(ISettingsService settings)
        {
            this.vapidDetails = new VapidDetails(settings.PushOwner, settings.PushPublicKey, settings.PushPrivateKey);
        }

        public async Task SendAsync(PushNotificationPayload payload, WebPushReceiver subscription)
        {
            var pushSubscription = new PushSubscription(subscription.Endpoint, subscription.P256DH, subscription.Auth);
            var jsonPayload = JsonSerializer.Serialize(payload, Constants.CamelCaseSerialization);

            using (var client = new WebPushClient())
            {
                await client.SendNotificationAsync(pushSubscription, jsonPayload, this.vapidDetails);
            }
        }
    }
}
