using System;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public SubscriptionMode Mode { get; set; } = SubscriptionMode.Once;
        public Guid AvatarId { get; set; }
        public Guid UserId { get; set; }
    }

    public enum SubscriptionMode
    {
        Once,
        Always
    }
}
