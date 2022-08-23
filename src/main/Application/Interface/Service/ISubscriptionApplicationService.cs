using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Common.Receivers;
using ei8.Cortex.Subscriptions.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Interface.Service
{
    public interface ISubscriptionApplicationService
    {
        Task AddSubscriptionAsync(SubscriptionInfo subscriptionInfo, IReceiverInfo receiverInfo);

        Task NotifySubscribers(AvatarUrlSnapshot avatarUrlSnapshot);
    }
}
