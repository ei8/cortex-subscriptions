namespace ei8.Cortex.Subscriptions.Application.Interface.Service
{
    public interface ISettingsService
    {
        string SubscriptionsDatabasePath { get; set; }
        int SubscriptionsPollingIntervalSeconds { get; set; }
    }
}
