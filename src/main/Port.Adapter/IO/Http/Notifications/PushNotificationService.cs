using ei8.Cortex.Subscriptions.Domain.Model;
using System.Text.Json;
using System.Threading.Tasks;
using WebPush;

namespace ei8.Cortex.Subscriptions.IO.Http.Notifications
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly string publicKey;
        private readonly string privateKey;
        private readonly string subject;

        private static readonly JsonSerializerOptions CamelCaseSerialization = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public PushNotificationService(ISettingsService settings)
        {
            //publicKey = settings.PushPublicKey;
            //privateKey = settings.PushPrivateKey;

            var keys = VapidHelper.GenerateVapidKeys();

            publicKey = keys.PublicKey;
            privateKey = keys.PrivateKey;

            subject = settings.PushOwner;
        }

        public async Task SendAsync(PushNotificationPayload payload, WebPushReceiver subscription)
        {
            var pushSubscription = new PushSubscription(subscription.Endpoint, subscription.P256DH, subscription.Auth);
            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
            var jsonPayload = JsonSerializer.Serialize(payload, CamelCaseSerialization);

            using (var client = new WebPushClient())
            {
                await client.SendNotificationAsync(pushSubscription, jsonPayload, vapidDetails);
            }
        }
    }
}
