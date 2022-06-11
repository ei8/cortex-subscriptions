using ei8.Cortex.Subscriptions.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application.Interface
{
    public interface IAvatarRepository
    {
        Task<Avatar> GetOrAddAsync(string url);
    }
}
