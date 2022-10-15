using ei8.Cortex.Subscriptions.Common;
using ei8.Net.Notifications;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications
{
    public interface INotificationTemplateApplicationService<out T> where T: INotificationPayload
    {
        T CreateNotificationPayload(NotificationTemplate templateType, Dictionary<string, object> templateValues);
    }
}
