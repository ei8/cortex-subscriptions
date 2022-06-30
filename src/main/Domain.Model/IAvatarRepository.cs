using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface IAvatarRepository
    {
        Task<Avatar> GetOrAddAsync(string url);

        Task<IList<Avatar>> GetAll();

        Task UpdateAsync(Avatar avatar);
    }
}
