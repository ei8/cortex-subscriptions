using ei8.Cortex.Subscriptions.Application.Interface.Repository;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite
{
    public class UserRepository : IUserRepository
    {
        private SQLiteAsyncConnection connection;

        public UserRepository()
        {
            connection = new SQLiteAsyncConnection(@"C:\Users\Junvic\Documents\avatar-server-pack\avatars\prod\sample\subscriptions.db");
        }

        public async Task<User> GetOrAddAsync(Guid id)
        {
            var user = await connection.Table<UserModel>().FirstOrDefaultAsync(u => u.UserNeuronId == id);

            if (user == null)
            {
                user = new UserModel()
                {
                    UserNeuronId = id
                };

                await connection.InsertAsync(user);
            }

            // TODO: Consider using AutoMapper to map from DB to domain model
            return new User()
            {
                UserNeuronId = id
            };
        }
    }
}
