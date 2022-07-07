using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;

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
            var model = new SubscriptionModel()
            {
                AvatarId = subscription.AvatarId,
                UserId = subscription.UserId,
                Id = subscription.Id
            };

            await this.connection.InsertAsync(model);
        }

        public async Task<IList<Subscription>> GetAllByAvatarIdAsync(Guid avatarId)
        {
            var subscriptions = (await this.connection.Table<SubscriptionModel>()
                                                      .ToListAsync())
                                                      .Where(s => s.AvatarId == avatarId);

            return subscriptions.Select(s => new Subscription()
            {
                Id = s.Id,
                UserId = s.UserId,
                AvatarId = s.AvatarId
            }).ToList();
        }

        public async Task<IList<Subscription>> GetAllByUserIdAsync(Guid userId)
        {
            var subscriptions = (await this.connection.Table<SubscriptionModel>()
                                                      .ToListAsync())
                                                      .Where(s => s.UserId == userId);

            return subscriptions.Select(s => new Subscription()
            {
                Id = s.Id,
                UserId = s.UserId,
                AvatarId = s.AvatarId
            }).ToList();
        }
    }
}
