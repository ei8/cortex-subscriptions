using ei8.Cortex.Subscriptions.Application.Interface;
using ei8.Cortex.Subscriptions.Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class AvatarRepositoryMock : IAvatarRepository
    {
        private List<Avatar> avatars = new List<Avatar>();

        public async Task<Avatar> GetOrAddAsync(string url)
        {
            var avatar = avatars.FirstOrDefault(s => s.Url == url);

            if (avatar == null)
            {
                avatar = new Avatar()
                {
                    Url = url
                };

                avatars.Add(avatar);
            }

            return avatar;
        }
    }
}
