using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Common;
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
        private readonly IAvatarRepository avatarRepository;
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly IUserRepository userRepository;
        private readonly IBrowserReceiverRepository browserReceiverRepository;
        private readonly IPushNotificationService notificationService;
        private readonly ILogger<SubscriptionApplicationService> logger;

        public SubscriptionApplicationService(IAvatarRepository avatarRepository, 
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository,
            IBrowserReceiverRepository browserReceiverRepository,
            IPushNotificationService notificationService,
            ILogger<SubscriptionApplicationService> logger)
        {
            this.avatarRepository = avatarRepository;
            this.subscriptionRepository = subscriptionRepository;
            this.userRepository = userRepository;
            this.browserReceiverRepository = browserReceiverRepository;
            this.notificationService = notificationService;
            this.logger = logger;
        }

        public async Task AddSubscriptionForBrowserAsync(BrowserSubscriptionInfo subscriptionInfo)
        {
            var user = await this.userRepository.GetOrAddAsync(subscriptionInfo.UserId);
            var avatar = await this.avatarRepository.GetOrAddAsync(subscriptionInfo.AvatarUrl);

            var receiver = new BrowserReceiver()
            {
                Id = Guid.NewGuid(),
                Name = subscriptionInfo.Name,
                PushAuth = subscriptionInfo.PushAuth,
                PushEndpoint = subscriptionInfo.PushEndpoint,
                PushP256DH = subscriptionInfo.PushP256DH,
                User = user
            };

            await this.browserReceiverRepository.AddAsync(receiver);

            var subscription = new Subscription()
            {
                AvatarId = avatar.Id,
                UserId = user.UserNeuronId,
                Id = Guid.NewGuid()
            };

             await this.subscriptionRepository.AddAsync(subscription);
        }

        public async Task<IList<Subscription>> GetAllByUserIdAsync(Guid userId)
        {
            return await this.subscriptionRepository.GetAllByUserIdAsync(userId);  
        }

        public async Task NotifySubscribers(Avatar avatar)
        {
            var notification = new PushNotificationPayload()
            {
                Title = "Avatar update",
                Body = $"Avatar changed: {avatar.Url}"
            };

            var subscriptions = await this.subscriptionRepository.GetAllByAvatarIdAsync(avatar.Id);

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
