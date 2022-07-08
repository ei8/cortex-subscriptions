using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Net.Http.PayloadHashing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class PollingApplicationService : IPollingApplicationService
    {
        private readonly IAvatarUrlSnapshotRepository avatarUrlSnapshotRepository;
        private readonly IPayloadHashService pollingService;

        public PollingApplicationService(IAvatarUrlSnapshotRepository avatarUrlSnapshotRepository, IPayloadHashService pollingService)
        {
            this.avatarUrlSnapshotRepository = avatarUrlSnapshotRepository;
            this.pollingService = pollingService;
        }

        public async Task<bool> CheckForChangesAsync(AvatarUrlSnapshot avatarUrlSnapshot)
        {
            var newHash = await this.pollingService.GetPayloadHashAsync(avatarUrlSnapshot.Url);

            if (newHash != avatarUrlSnapshot.Hash)
            {
                avatarUrlSnapshot.Hash = newHash;
                await this.avatarUrlSnapshotRepository.UpdateAsync(avatarUrlSnapshot);

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IList<AvatarUrlSnapshot>> GetAvatarUrlsForPollingAsync()
        {
            return await this.avatarUrlSnapshotRepository.GetAll();
        }
    }
}
