using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Net.Http.Notifications;
using ei8.Net.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public class WebPushNotificationApplicationService : INotificationApplicationService
    {
        private readonly IBrowserReceiverRepository repository;
        private readonly INotificationService<WebPushNotificationPayload, WebPushReceiver> pushNotificationService;
        private readonly ILogger<WebPushNotificationApplicationService> logger;

        public WebPushNotificationApplicationService(IBrowserReceiverRepository repository,
            INotificationService<WebPushNotificationPayload, WebPushReceiver> pushNotificationService,
            ILogger<WebPushNotificationApplicationService> logger)
        {
            this.repository = repository;
            this.pushNotificationService = pushNotificationService;
            this.logger = logger;
        }

        public async Task NotifyReceiversForUserAsync(Guid userNeuronId, string avatarUrl)
        {
            var notification = new WebPushNotificationPayload()
            {
                Title = "Avatar update",
                Body = $"Avatar changed: {avatarUrl}"
            };

            var receivers = await this.repository.GetByUserIdAsync(userNeuronId);

            foreach (var r in receivers)
            {
                await this.TrySendWebNotification(notification, r);
            }
        }
        private async Task TrySendWebNotification(WebPushNotificationPayload notification, BrowserReceiver receiver)
        {
            try
            {
                var pushReceiver = new WebPushReceiver()
                {
                    Endpoint = receiver.PushEndpoint,
                    P256DH = receiver.PushP256DH,
                    Auth = receiver.PushAuth
                };

                await this.pushNotificationService.SendAsync(notification, pushReceiver);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error sending push notification: {Message}", ex.Message);
            }
        }
    }
}
