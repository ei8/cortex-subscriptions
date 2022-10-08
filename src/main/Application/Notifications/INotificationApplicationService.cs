using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public interface INotificationApplicationService
    {
        Task NotifyReceiversForUserAsync(Guid userNeuronId, string avatarUrl);
    }
}
