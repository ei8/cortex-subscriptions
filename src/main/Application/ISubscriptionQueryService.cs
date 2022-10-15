using ei8.Cortex.Subscriptions.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public interface ISubscriptionQueryService
    {
        Task<ServerConfiguration> GetServerConfigurationAsync();
    }
}
