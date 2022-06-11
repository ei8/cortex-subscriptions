using ei8.Cortex.Subscriptions.Domain.Model;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Interface
{
    public interface IUserRepository
    {
        Task<User> GetOrAddAsync(Guid id);
    }
}
