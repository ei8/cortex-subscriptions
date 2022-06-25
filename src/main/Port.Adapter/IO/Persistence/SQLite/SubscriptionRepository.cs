using ei8.Cortex.Subscriptions.Application.Interface.Repository;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Extensions;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private SQLiteAsyncConnection connection;

        public SubscriptionRepository()
        {
            connection = new SQLiteAsyncConnection(@"C:\Users\Junvic\Documents\avatar-server-pack\avatars\prod\sample\subscriptions.db");
        }

        public async Task AddAsync(Subscription subscription)
        {
            var model = new SubscriptionModel()
            {
                AvatarId = subscription.Avatar.Id,
                UserId = subscription.User.UserNeuronId,
                Id = subscription.Id
            };

            await connection.InsertAsync(model);
        }

        public async Task<IList<Subscription>> GetAllForUserIdAsync(Guid userId)
        {
            var subscriptions = await connection.GetAllWithChildren<SubscriptionModel>(s => s.UserId == userId, recursive: true);

            return subscriptions.Select(s => new Subscription()
            {
                Id = s.Id,
                User = new User() { UserNeuronId = s.UserId },
                Avatar = new Avatar()
                {
                    Hash = s.Avatar.Hash,
                    Id = s.Avatar.Id,
                    Url = s.Avatar.Url
                }
            }).ToList();
        }
    }
}
