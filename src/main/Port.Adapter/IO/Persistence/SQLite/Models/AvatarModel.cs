using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models
{
    [Table("Avatar")]
    internal class AvatarModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Hash { get; set; }   
        public string Url { get; set; }
    }
}
