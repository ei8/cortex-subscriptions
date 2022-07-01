using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface ISubscriptionRepository    
    {
        Task<IList<Subscription>> GetAllByUserIdAsync(Guid userId);
        Task<IList<Subscription>> GetAllByAvatarIdAsync(Guid avatarId);
        Task AddAsync(Subscription subscription);
    }
}
