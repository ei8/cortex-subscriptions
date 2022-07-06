using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.IO.Http.PayloadHashing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class PollingApplicationService : IPollingApplicationService
    {
        private readonly IAvatarRepository avatarRepository;
        private readonly IPayloadHashService pollingService;

        public PollingApplicationService(IAvatarRepository avatarRepository, IPayloadHashService pollingService)
        {
            this.avatarRepository = avatarRepository;
            this.pollingService = pollingService;
        }

        public async Task<bool> CheckForChangesAsync(Avatar avatar)
        {
            var newHash = await this.pollingService.GetPayloadHashAsync(avatar.Url);

            if (newHash != avatar.Hash)
            {
                avatar.Hash = newHash;
                await this.avatarRepository.UpdateAsync(avatar);

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IList<Avatar>> GetAvatarsForPollingAsync()
        {
            return await this.avatarRepository.GetAll();
        }
    }
}
