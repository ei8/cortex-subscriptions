using ei8.Cortex.Subscriptions.Application.PushNotifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Net.Email.Smtp.Notifications;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public class SmtpNotificationTemplateApplicationService : BaseTemplateApplicationService, INotificationTemplateApplicationService<SmtpNotificationPayload>
    {
        public SmtpNotificationTemplateApplicationService(ISettingsService settingsService) : base(settingsService)
        {
        }

        public SmtpNotificationPayload CreateNotificationPayload(NotificationTemplate templateType, Dictionary<string, object> templateValues)
        {
            base.SetDefaultAvatarUrlParameter(templateValues);

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
                        Body = $"Hi {receiverName}, You have received a new request to access restricted neurons within your avatar. Please visit {avatarUrl} to manage the request."
                    };

                default:
                    throw new NotSupportedException($"Unsupported template type {templateType}");
            }
        }
    }
}
