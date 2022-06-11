using ei8.Cortex.Subscriptions.Application.Interface;
using ei8.Cortex.Subscriptions.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class BrowserReceiverRepositoryMock : IBrowserReceiverRepository
    {
        private List<BrowserReceiver> browserReceivers = new List<BrowserReceiver>();

        public async Task AddAsync(BrowserReceiver receiver)
        {
            browserReceivers.Add(receiver);
        }

        public async Task<IList<BrowserReceiver>> GetByUserIdAsync(Guid id)
        {
            return browserReceivers.Where(r => r.User.NeuronId == id).ToList();
        }
    }
}
