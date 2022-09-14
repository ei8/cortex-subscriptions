using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications
{
    public interface IPushNotificationApplicationService
    {
        Task NotifyReceiversForUserAsync(Guid userNeuronId, string title, string body);
    }
}
