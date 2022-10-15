using ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Net.Http.Notifications;
using ei8.Net.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public class WebPushNotificationApplicationService : INotificationApplicationService
    {
        private readonly IBrowserReceiverRepository repository;
        private readonly INotificationService<WebPushNotificationPayload, WebPushReceiver> pushNotificationService;
        private readonly ILogger<WebPushNotificationApplicationService> logger;
        private readonly INotificationTemplateApplicationService<WebPushNotificationPayload> templateApplicationService;

        public WebPushNotificationApplicationService(IBrowserReceiverRepository repository,
            INotificationService<WebPushNotificationPayload, WebPushReceiver> pushNotificationService,
            ILogger<WebPushNotificationApplicationService> logger,
            INotificationTemplateApplicationService<WebPushNotificationPayload>)
        {
            this.repository = repository;
            this.pushNotificationService = pushNotificationService;
            this.logger = logger;
            this.templateApplicationService = templateApplicationService;
        }

        public async Task NotifyReceiversForUserAsync(Guid userNeuronId, NotificationTemplate templateType, Dictionary<string, object> templateValues)
        {
            var notification = this.templateApplicationService.CreateNotificationPayload(templateType, templateValues);
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
