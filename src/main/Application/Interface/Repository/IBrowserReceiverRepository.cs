using ei8.Cortex.Subscriptions.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Interface.Repository
{
    public interface IBrowserReceiverRepository
    {
        Task<IList<BrowserReceiver>> GetByUserIdAsync(Guid id);

        Task AddAsync(BrowserReceiver receiver);
    }
}
