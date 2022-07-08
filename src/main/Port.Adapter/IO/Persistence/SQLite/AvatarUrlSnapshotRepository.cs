using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Extensions;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models;
using SQLite;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite
{
    public class AvatarUrlSnapshotRepository : IAvatarUrlSnapshotRepository
    {
        private readonly SQLiteAsyncConnection connection;

        public AvatarUrlSnapshotRepository(ISettingsService settings)
        {
            this.connection = new SQLiteAsyncConnection(settings.DatabasePath);
        }

        public async Task<IList<AvatarUrlSnapshot>> GetAll()
        {
            var rows = await this.connection.GetAllWithChildren<AvatarModel>(recursive: true);

            return rows.Select(r => new AvatarUrlSnapshot()
            {
                Hash = r.Hash,
                Id = r.Id,
                Url = r.Url
            }).ToList();
        }

        public async Task<AvatarUrlSnapshot> GetOrAddAsync(string url)
        {
            var avatar = await this.connection.Table<AvatarModel>()
                                              .FirstOrDefaultAsync(t => t.Url == url);

            if (avatar == null)
            {
                avatar = new AvatarModel()
                {
                    Url = url,
                    Id = Guid.NewGuid()
                };

                await this.connection.InsertAsync(avatar);
            }

            return new AvatarUrlSnapshot()
            {
                Hash = avatar.Hash,
                Id = avatar.Id,
                Url = avatar.Url
            };
        }

        public async Task UpdateAsync(AvatarUrlSnapshot avatar)
        {
            var existingAvatar = await this.connection.Table<AvatarModel>()
                                                      .FirstOrDefaultAsync(t => t.Url == avatar.Url);

            if (existingAvatar != null)
            {
                existingAvatar.Hash = avatar.Hash;
                await this.connection.UpdateAsync(existingAvatar);
            }
        }
    }
}