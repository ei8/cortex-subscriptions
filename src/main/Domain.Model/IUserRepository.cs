using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface IUserRepository
    {
        Task<User> GetOrAddAsync(Guid id);
    }
}
