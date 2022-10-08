using ei8.Cortex.Subscriptions.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class SubscriptionQueryService : ISubscriptionQueryService
    {
        private readonly ISettingsService settings;

        public SubscriptionQueryService(ISettingsService settings)
        {
            this.settings = settings;
        }

        public async Task<ServerConfiguration> GetServerConfigurationAsync()
        {
            var config = new ServerConfiguration()
            {
                ServerPublicKey = this.settings.PushSettings.PushPublicKey
            };

            return config;
        }
    }
}
