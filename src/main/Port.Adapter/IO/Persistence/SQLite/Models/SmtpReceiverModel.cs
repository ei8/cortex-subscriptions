using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite.Models
{
    [Table("SmtpReceiver")]
    internal class SmtpReceiverModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(UserModel))]
        public Guid UserNeuronId { get; set; }

        public string Address { get; set; }
    }
}
