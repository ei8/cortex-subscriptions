using System;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public class User
    {
        public Guid UserNeuronId { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Receiver> NotificationReceivers { get; set; } = new List<Receiver>();
    }
}
