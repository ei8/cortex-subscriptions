using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models
{
    [Table("Subscription")]
    internal class SubscriptionModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(AvatarModel))]
        public Guid AvatarId { get; set; }

        [ForeignKey(typeof(UserModel))]
        public Guid UserId { get; set; }

        [ManyToOne]
        public UserModel User { get; set; }
    }
}
