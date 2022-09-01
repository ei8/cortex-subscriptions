using System;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public SubscriptionMode Mode { get; set; } = SubscriptionMode.Once;
        public Guid AvatarUrlSnapshotId { get; set; }
        public Guid UserNeuronId { get; set; }
    }

    public enum SubscriptionMode
    {
        Once,
        Always
    }
}
