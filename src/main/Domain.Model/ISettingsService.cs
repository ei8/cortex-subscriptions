namespace ei8.Cortex.Subscriptions.Domain.Model
{
    public interface ISettingsService
    {
        string DatabasePath { get; set; }
        int PollingIntervalSeconds { get; set; }
        string PushOwner { get; set; }
        string PushPublicKey { get; set; }
        string PushPrivateKey { get; set; }
    }
}
