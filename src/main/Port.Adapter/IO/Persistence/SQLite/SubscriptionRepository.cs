using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private SQLiteAsyncConnection connection;

        public SubscriptionRepository(ISettingsService settings)
        {
            this.connection = new SQLiteAsyncConnection(settings.DatabasePath);
        }

        public async Task AddAsync(Subscription subscription)
        {
            // check if existing subscription for the user and avatar ID pair exist
            var existingSubscription = await this.connection.Table<SubscriptionModel>()
                                                            .FirstOrDefaultAsync(s => s.AvatarId == subscription.AvatarUrlSnapshotId && s.UserNeuronId == subscription.UserNeuronId);

            if (existingSubscription == null)
            {
                var model = new SubscriptionModel()
                {
                    AvatarId = subscription.AvatarUrlSnapshotId,
                    UserNeuronId = subscription.UserNeuronId,
                    Id = subscription.Id
                };

                await this.connection.InsertAsync(model);
            }
        }

        public async Task<IList<Subscription>> GetAllByAvatarUrlSnapshotIdAsync(Guid avatarId)
        {
            var subscriptions = (await this.connection.Table<SubscriptionModel>()
                                                      .ToListAsync())
                                                      .Where(s => s.AvatarId == avatarId);

            return subscriptions.Select(s => new Subscription()
            {
                Id = s.Id,
                UserNeuronId = s.UserNeuronId,
                AvatarUrlSnapshotId = s.AvatarId
            }).ToList();
        }

        public async Task<IList<Subscription>> GetAllByUserIdAsync(Guid userId)
        {
            var subscriptions = (await this.connection.Table<SubscriptionModel>()
                                                      .ToListAsync())
                                                      .Where(s => s.UserNeuronId == userId);

            return subscriptions.Select(s => new Subscription()
            {
                Id = s.Id,
                UserNeuronId = s.UserNeuronId,
                AvatarUrlSnapshotId = s.AvatarId
            }).ToList();
        }
    }
}
