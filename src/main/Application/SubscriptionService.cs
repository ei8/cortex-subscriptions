using ei8.Cortex.Subscriptions.Application.Interface.Repository;
using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IAvatarRepository avatarRepository;
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly IUserRepository userRepository;
        private readonly IBrowserReceiverRepository browserReceiverRepository;

        public SubscriptionService(IAvatarRepository avatarRepository, 
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository,
            IBrowserReceiverRepository browserReceiverRepository)
        {
            this.avatarRepository = avatarRepository;
            this.subscriptionRepository = subscriptionRepository;
            this.userRepository = userRepository;
            this.browserReceiverRepository = browserReceiverRepository;
        }

        public async Task AddSubscriptionForBrowserAsync(BrowserSubscriptionInfo subscriptionInfo)
        {
            var user = await userRepository.GetOrAddAsync(subscriptionInfo.UserId);
            var avatar = await avatarRepository.GetOrAddAsync(subscriptionInfo.AvatarUrl);

            var receiver = new BrowserReceiver()
            {
                Id = Guid.NewGuid(),
                Name = subscriptionInfo.Name,
                PushAuth = subscriptionInfo.PushAuth,
                PushEndpoint = subscriptionInfo.PushEndpoint,
                PushP256DH = subscriptionInfo.PushP256DH,
                User = user
            };

            await browserReceiverRepository.AddAsync(receiver);

            var subscription = new Subscription()
            {
                Avatar = avatar,
                User = user,
                Id = Guid.NewGuid()
            };

             await subscriptionRepository.AddAsync(subscription);
        }

        public async Task<IList<Subscription>> GetSubscriptionsForUserAsync(Guid userId)
        {
            return await subscriptionRepository.GetAllForUserIdAsync(userId);  
        }
    }
}
