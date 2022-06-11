using ei8.Cortex.Subscriptions.Application.Interface;
using ei8.Cortex.Subscriptions.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Application
{
    public class UserRepositoryMock : IUserRepository
    {
        private List<User> users = new List<User>();

        public async Task<User> GetOrAddAsync(Guid id)
        {
            var existingUser = users.FirstOrDefault(x => x.NeuronId == id);

            if (existingUser == null)
            {
                existingUser = new User()
                {
                    NeuronId = id,
                };
                users.Add(existingUser);
            }

            return existingUser;
        }
    }
}
