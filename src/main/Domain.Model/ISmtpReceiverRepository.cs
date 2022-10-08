using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface ISmtpReceiverRepository
    {
        Task<IList<SmtpReceiver>> GetByUserIdAsync(Guid id);

        Task AddAsync(SmtpReceiver receiver);
    }
}
