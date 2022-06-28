namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public class BrowserReceiver : Receiver
    {
        public string Name { get; set; }
        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        public string PushAuth { get; set; }
    }
}
