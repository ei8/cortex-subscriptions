using System;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public abstract class Receiver
    {
        public User User { get; set; }
        public Guid Id { get; set; }
    }
}
