using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface IAvatarUrlSnapshotRepository
    {
        Task<AvatarUrlSnapshot> GetOrAddAsync(string url);

        Task<IList<AvatarUrlSnapshot>> GetAll();

        Task UpdateAsync(AvatarUrlSnapshot avatarUrlSnapshot);
    }
}
