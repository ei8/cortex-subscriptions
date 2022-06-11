using System;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public class User
    {
        public Guid NeuronId { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Receiver> NotificationReceivers { get; set; }
    }
}
