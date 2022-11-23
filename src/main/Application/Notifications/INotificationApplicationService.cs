using ei8.Cortex.Subscriptions.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public interface INotificationApplicationService
    {
        Task NotifyReceiversForUserAsync(Guid userNeuronId, NotificationTemplate templateType, Dictionary<string, object> templateValues);
    }
}
