using SQLite;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models
{
    [Table("User")]
    internal class UserModel
    {
        [PrimaryKey]
        public Guid UserNeuronId { get; set; }
    }
}
