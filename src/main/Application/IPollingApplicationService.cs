using ei8.Cortex.Subscriptions.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public interface IPollingApplicationService
    {
        Task<IList<AvatarUrlSnapshot>> GetAvatarUrlsForPollingAsync();
        Task<bool> CheckForChangesAsync(AvatarUrlSnapshot avatarUrlSnapshot);
    }
}
