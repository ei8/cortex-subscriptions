using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Extensions;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite
{
    public class AvatarRepository : IAvatarRepository
    {
        private readonly SQLiteAsyncConnection connection;

        public AvatarRepository(ISettingsService settings)
        {
            connection = new SQLiteAsyncConnection(settings.DatabasePath);
        }

        public async Task<IList<Avatar>> GetAll()
        {
            var rows = await connection.GetAllWithChildren<AvatarModel>(recursive: true);

            return rows.Select(r => new Avatar()
            {
                Hash = r.Hash,
                Id = r.Id,
                Url = r.Url
            }).ToList();
        }

        public async Task<Avatar> GetOrAddAsync(string url)
        {
            var avatar = await connection.Table<AvatarModel>().FirstOrDefaultAsync(t => t.Url == url);

            if (avatar == null)
            {
                avatar = new AvatarModel()
                {
                    Url = url,
                    Id = Guid.NewGuid()
                };

                await connection.InsertAsync(avatar);
            }

            return new Avatar()
            {
                Hash = avatar.Hash,
                Id = avatar.Id,
                Url = avatar.Url
            };
        }

        public async Task UpdateAsync(Avatar avatar)
        {
            var existingAvatar = await connection.Table<AvatarModel>().FirstOrDefaultAsync(t => t.Url == avatar.Url);

            if (existingAvatar != null)
            {
                existingAvatar.Hash = avatar.Hash;
                await connection.UpdateAsync(existingAvatar);
            }
        }
    }
}