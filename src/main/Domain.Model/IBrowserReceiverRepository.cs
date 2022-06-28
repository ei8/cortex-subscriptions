using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface IBrowserReceiverRepository
    {
        Task<IList<BrowserReceiver>> GetByUserIdAsync(Guid id);

        Task AddAsync(BrowserReceiver receiver);
    }
}
