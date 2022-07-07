using System;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public class Avatar
    {
        public string Hash { get; set; }
        public string Url { get; set; }
        public Guid Id { get; set; }
    }
}
