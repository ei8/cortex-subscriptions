using ei8.Cortex.Graph.Client;
using ei8.Cortex.Subscriptions.Application.PushNotifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Net.Email.Smtp.Notifications;
using ei8.Net.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public class SmtpNotificationApplicationService : INotificationApplicationService
    {
        private readonly ISmtpReceiverRepository repository;
        private readonly INotificationService<SmtpNotificationPayload, Net.Email.Smtp.Notifications.SmtpReceiver> notificationService;
        private readonly INeuronGraphQueryClient neuronGraphQueryClient;
        private readonly ISettingsService settingsService;
        private readonly ILogger<SmtpNotificationApplicationService> logger;
        private readonly INotificationTemplateApplicationService<SmtpNotificationPayload> templateApplicationService;

        public SmtpNotificationApplicationService(
            ISmtpReceiverRepository repository,
            INotificationService<SmtpNotificationPayload, Net.Email.Smtp.Notifications.SmtpReceiver> notificationService,
            INeuronGraphQueryClient neuronGraphQueryClient,
            ISettingsService settingsService,
            ILogger<SmtpNotificationApplicationService> logger,
            INotificationTemplateApplicationService<SmtpNotificationPayload> templateApplicationService
            )
        {
            this.repository = repository;
            this.notificationService = notificationService;
            this.logger = logger;
            this.templateApplicationService = templateApplicationService;
            this.neuronGraphQueryClient = neuronGraphQueryClient;
            this.settingsService = settingsService;
        }

        public async Task NotifyReceiversForUserAsync(Guid userNeuronId, NotificationTemplate templateType, Dictionary<string, object> templateValues)
        {
            var queryResult = (await this.neuronGraphQueryClient.GetNeuronById(
                this.settingsService.CortexGraphOutBaseUrl + "/",
                userNeuronId.ToString(),
                new Graph.Common.NeuronQuery() { NeuronActiveValues = Graph.Common.ActiveValues.All }
                ));
            var receiverName = queryResult.Neurons.FirstOrDefault()?.Tag;

            templateValues.Add(NotificationTemplateParameters.ReceiverName, receiverName);

            var notification = this.templateApplicationService.CreateNotificationPayload(templateType, templateValues);
            var receivers = await this.repository.GetByUserIdAsync(userNeuronId);

            foreach (var r in receivers)
            {
                await this.TrySendSmtpNotification(notification, r, receiverName);
            }
        }

        private async Task TrySendSmtpNotification(SmtpNotificationPayload notification, Domain.Model.SmtpReceiver receiver, string receiverName)
        {
            try
            {
                var pushReceiver = new ei8.Net.Email.Smtp.Notifications.SmtpReceiver()
                {
                    Address = receiver.Address,
                    Name = receiverName
                };

                await this.notificationService.SendAsync(notification, pushReceiver);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error sending push notification: {Message}", ex.Message);
            }
        }
    }
}
