using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Common.Receivers;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Net.Http.Notifications;
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
        private readonly IPushNotificationService notificationService;
        private readonly ILogger<SubscriptionApplicationService> logger;

        public SubscriptionApplicationService(IAvatarUrlSnapshotRepository avatarUrlSnapshotRepository, 
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository,
            IBrowserReceiverRepository browserReceiverRepository,
            IPushNotificationService notificationService,
            ILogger<SubscriptionApplicationService> logger)
        {
            this.avatarUrlSnapshotRepository = avatarUrlSnapshotRepository;
            this.subscriptionRepository = subscriptionRepository;
            this.userRepository = userRepository;
            this.browserReceiverRepository = browserReceiverRepository;
            this.notificationService = notificationService;
            this.logger = logger;
        }

        public async Task AddSubscriptionAsync(SubscriptionInfo subscriptionInfo, IReceiverInfo receiverInfo)
        {
            var user = await this.userRepository.GetOrAddAsync(subscriptionInfo.UserId);
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
                UserId = user.UserNeuronId,
                Id = Guid.NewGuid()
            };

            await this.subscriptionRepository.AddAsync(subscription);
        }

        public async Task<IList<Subscription>> GetAllByUserIdAsync(Guid userId)
        {
            return await this.subscriptionRepository.GetAllByUserIdAsync(userId);  
        }

        public async Task NotifySubscribers(AvatarUrlSnapshot avatarUrlSnapshot)
        {
            var notification = new PushNotificationPayload()
            {
                Title = "Avatar update",
                Body = $"Avatar changed: {avatarUrlSnapshot.Url}"
            };

            var subscriptions = await this.subscriptionRepository.GetAllByAvatarUrlSnapshotIdAsync(avatarUrlSnapshot.Id);

            foreach (var sub in subscriptions)
            {
                var receivers = await this.browserReceiverRepository.GetByUserIdAsync(sub.UserId);

                foreach (var r in receivers)
                {
                    await TrySendNotification(notification, r);
                }
            }
        }

        private async Task TrySendNotification(PushNotificationPayload notification, BrowserReceiver receiver)
        {
            try
            {
                var pushReceiver = new WebPushReceiver()
                {
                    Endpoint = receiver.PushEndpoint,
                    P256DH = receiver.PushP256DH,
                    Auth = receiver.PushAuth
                };

                await this.notificationService.SendAsync(notification, pushReceiver);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error sending push notification: {Message}", ex.Message);
            }
        }
    }
}
