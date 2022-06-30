using ei8.Cortex.Subscriptions.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Interface.Service
{
    public interface IPollingApplicationService
    {
        Task<IList<Avatar>> GetAvatarsForPollingAsync();
        Task<bool> CheckForChangesAsync(Avatar avatar);
    }
}
