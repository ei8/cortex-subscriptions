using ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications;
using ei8.Cortex.Subscriptions.Application.Notifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Net.Http.Notifications;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Application.PushNotifications
{
    public class WebPushTemplateApplicationService : BaseTemplateApplicationService, INotificationTemplateApplicationService<WebPushNotificationPayload>
    {
        public WebPushTemplateApplicationService(ISettingsService settingsService) : base(settingsService)
        {
        }

        public WebPushNotificationPayload CreateNotificationPayload(NotificationTemplate templateType, Dictionary<string, object> templateValues)
        {
            base.SetDefaultAvatarUrlParameter(templateValues);
            object avatarUrl;

            switch (templateType)
            {
                case NotificationTemplate.AvatarUpdated:
                    _ = templateValues.TryGetValue(NotificationTemplateParameters.AvatarUrl, out avatarUrl);
                    return new WebPushNotificationPayload()
                    {
                        Title = "Avatar updated",
                        Body = $"Avatar URL: {avatarUrl.ToString()}"
                    };

                case NotificationTemplate.NeuronAccessRequested:
                    _ = templateValues.TryGetValue(NotificationTemplateParameters.AvatarUrl, out avatarUrl);
                    return new WebPushNotificationPayload()
                    {
                        Title = "New access request",
                        Body = $"Access request received for restricted neurons. Click to manage request."
                    };

                default:
                    throw new NotSupportedException($"Unsupported template type {templateType}");
            }
        }
    }
}
