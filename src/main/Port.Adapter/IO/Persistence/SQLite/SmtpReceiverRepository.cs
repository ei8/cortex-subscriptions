using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite
{
    public class SmtpReceiverRepository : ISmtpReceiverRepository
    {
        private readonly SQLiteAsyncConnection connection;

        public SmtpReceiverRepository(ISettingsService settings)
        {
            this.connection = new SQLiteAsyncConnection(settings.DatabasePath);
        }

        public async Task AddAsync(SmtpReceiver receiver)
        {
            // check if already exists
            var existingReceiverCount = await this.connection.Table<SmtpReceiverModel>()
                .CountAsync(b => b.UserNeuronId == receiver.UserNeuronId &&
                                 b.Address == receiver.Address
                                 );

            if (existingReceiverCount > 0)
                throw new ArgumentException($"Specified receiver '{receiver.Address}' already exists.");

            var newReceiver = new SmtpReceiverModel()
            {
                Id = receiver.Id,
                UserNeuronId = receiver.UserNeuronId,
                Address = receiver.Address 
            };

            await this.connection.InsertAsync(newReceiver);
        }

        public async Task<IList<SmtpReceiver>> GetByUserIdAsync(Guid id)
        {
            var user = new User()
            {
                UserNeuronId = (await this.connection.Table<UserModel>().FirstAsync(u => u.UserNeuronId == id)).UserNeuronId
            };

            var list = await this.connection.Table<SmtpReceiverModel>()
                                            .Where(b => b.UserNeuronId == id)
                                            .ToListAsync();

            return list.Select(b => new SmtpReceiver()
            {
                Id = b.Id,
                Address = b.Address,
                UserNeuronId = b.UserNeuronId
            }).ToList();
        }
    }
}
