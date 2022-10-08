using System;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public abstract class Receiver
    {
        public Guid UserNeuronId { get; set; }
        public Guid Id { get; set; }
    }
}
