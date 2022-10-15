using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public class SmtpReceiver : Receiver
    {
        public string Address { get; set; }
    }
}
