using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models
{
    [Table("BrowserReceiver")]
    internal class BrowserReceiverModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(UserModel))]
        public Guid UserNeuronId { get; set; }

        public string Name { get; set; }
        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        public string PushAuth { get; set; }
    }
}
