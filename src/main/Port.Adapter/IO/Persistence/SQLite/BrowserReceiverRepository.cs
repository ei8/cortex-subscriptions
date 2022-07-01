using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite
{
    public class BrowserReceiverRepository : IBrowserReceiverRepository
    {
        private readonly SQLiteAsyncConnection connection;

        public BrowserReceiverRepository(ISettingsService settings)
        {
            connection = new SQLiteAsyncConnection(settings.DatabasePath);
        }

        public async Task AddAsync(BrowserReceiver receiver)
        {
            // check if already exists
            var existingReceiver = await connection.Table<BrowserReceiverModel>().FirstOrDefaultAsync(b => b.PushEndpoint == receiver.PushEndpoint &&
                                                                                                      b.PushP256DH == receiver.PushP256DH &&
                                                                                                      b.PushAuth == receiver.PushAuth);

            if (existingReceiver == null)
            {
                existingReceiver = new BrowserReceiverModel()
                {
                    Id = receiver.Id,
                    Name = receiver.Name,
                    UserId = receiver.User.UserNeuronId,
                    PushAuth = receiver.PushAuth,
                    PushEndpoint = receiver.PushEndpoint,
                    PushP256DH = receiver.PushP256DH,
                };

                await connection.InsertAsync(existingReceiver);
            }
        }

        public async Task<IList<BrowserReceiver>> GetByUserIdAsync(Guid id)
        {
            var user = new User()
            {
                UserNeuronId = (await connection.Table<UserModel>()
                                                .FirstAsync(u => u.UserNeuronId == id)).UserNeuronId
            };

            var list = await connection.Table<BrowserReceiverModel>()
                                       .Where(b => b.UserId == id)
                                       .ToListAsync();

            return list.Select(b => new BrowserReceiver()
            {
                Id = b.Id,
                Name = b.Name,
                PushAuth = b.PushAuth,
                PushEndpoint = b.PushEndpoint,
                PushP256DH = b.PushP256DH,
                User = user
            }).ToList();
        }
    }
}
