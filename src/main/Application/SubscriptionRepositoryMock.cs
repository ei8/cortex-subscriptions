using ei8.Cortex.Subscriptions.Application.Interface;
using ei8.Cortex.Subscriptions.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class SubscriptionRepositoryMock : ISubscriptionRepository
    {
        private List<Subscription> subscriptions = new List<Subscription>();

        public async Task AddAsync(Subscription subscription)
        {
            subscriptions.Add(subscription);
        }

        public async Task<IList<Subscription>> GetAllForUserIdAsync(Guid userId)
        {
            return subscriptions.Where(s => s.User.NeuronId == userId).ToList();
        }
    }
}
