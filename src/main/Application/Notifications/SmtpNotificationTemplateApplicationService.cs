using ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Net.Email.Smtp.Notifications;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public class SmtpNotificationTemplateApplicationService : INotificationTemplateApplicationService<SmtpNotificationPayload>
    {
        public SmtpNotificationPayload CreateNotificationPayload(NotificationTemplate templateType, Dictionary<string, object> templateValues)
        {
            object avatarUrl;
            object receiverName;

            switch (templateType)
            {
                case NotificationTemplate.AvatarUpdated:
                    _ = templateValues.TryGetValue(NotificationTemplateParameters.AvatarUrl, out avatarUrl);
                    _ = templateValues.TryGetValue(NotificationTemplateParameters.ReceiverName, out receiverName);
                    return new SmtpNotificationPayload()
                    {
                        Subject = "Avatar update",
                        Body = $"Hi {receiverName}, Avatar changed: {avatarUrl}"
                    };

                case NotificationTemplate.NeuronAccessRequested:
                    _ = templateValues.TryGetValue(NotificationTemplateParameters.AvatarUrl, out avatarUrl);
                    _ = templateValues.TryGetValue(NotificationTemplateParameters.ReceiverName, out receiverName);

                    return new SmtpNotificationPayload()
                    {
                        Subject = "New access request",
                        Body = $"Hi {receiverName}, You have received a new request to access restricted neurons within your avatar. Click <a href=\"{avatarUrl}\">here</a> to manage the request."
                    };

                default:
                    throw new NotSupportedException($"Unsupported template type {templateType}");
            }
        }
    }
}
