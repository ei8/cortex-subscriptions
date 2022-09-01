using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Common.Receivers;
using ei8.Cortex.Subscriptions.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class SubscriptionApplicationService : ISubscriptionApplicationService
    {
        private readonly IAvatarUrlSnapshotRepository avatarUrlSnapshotRepository;
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly IUserRepository userRepository;
        private readonly IBrowserReceiverRepository browserReceiverRepository;
        private readonly IEnumerable<IPushNotificationApplicationService> notificationServices;
        private readonly ILogger<SubscriptionApplicationService> logger;

        public SubscriptionApplicationService(IAvatarUrlSnapshotRepository avatarUrlSnapshotRepository, 
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository,
            IBrowserReceiverRepository browserReceiverRepository,
            IEnumerable<IPushNotificationApplicationService> notificationServices,
            ILogger<SubscriptionApplicationService> logger)
        {
            this.avatarUrlSnapshotRepository = avatarUrlSnapshotRepository;
            this.subscriptionRepository = subscriptionRepository;
            this.userRepository = userRepository;
            this.browserReceiverRepository = browserReceiverRepository;
            this.notificationServices = notificationServices;
            this.logger = logger;
        }

        public async Task AddSubscriptionAsync(SubscriptionInfo subscriptionInfo, IReceiverInfo receiverInfo)
        {
            var user = await this.userRepository.GetOrAddAsync(subscriptionInfo.UserNeuronId);
            var avatarUrlSnapshot = await this.avatarUrlSnapshotRepository.GetOrAddAsync(subscriptionInfo.AvatarUrl);

            switch (receiverInfo)
            {
                case BrowserReceiverInfo br:
                    var receiver = new BrowserReceiver()
                    {
                        Id = Guid.NewGuid(),
                        Name = br.Name,
                        PushAuth = br.PushAuth,
                        PushEndpoint = br.PushEndpoint,
                        PushP256DH = br.PushP256DH,
                        User = user
                    };
                    await this.browserReceiverRepository.AddAsync(receiver);
                    break;

                default:
                    throw new NotSupportedException($"Unsupported receiver info type: {receiverInfo.GetType()}");
            }

            var subscription = new Subscription()
            {
                AvatarUrlSnapshotId = avatarUrlSnapshot.Id,
                UserNeuronId = user.UserNeuronId,
                Id = Guid.NewGuid()
            };

            await this.subscriptionRepository.AddAsync(subscription);
        }

        public async Task NotifySubscribers(AvatarUrlSnapshot avatarUrlSnapshot)
        {
            var subscriptions = await this.subscriptionRepository.GetAllByAvatarUrlSnapshotIdAsync(avatarUrlSnapshot.Id);

            foreach (var sub in subscriptions)
            {
                foreach (var notificationService in this.notificationServices)
                {
                    await notificationService.NotifyReceiversForUserAsync(sub.UserNeuronId, avatarUrlSnapshot.Url);
                }
            }
        }
    }
}
