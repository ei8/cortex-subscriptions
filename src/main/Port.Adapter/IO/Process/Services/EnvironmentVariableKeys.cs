namespace ei8.Cortex.Subscriptions.Port.Adapter.IO.Process.Services
{
    internal sealed class EnvironmentVariableKeys
    {
        public const string SubscriptionsDatabasePath = "SUBSCRIPTIONS_DATABASE_PATH";
        public const string SubscriptionsPollingIntervalSeconds = "SUBSCRIPTIONS_POLLING_INTERVAL_SECS";
        public const string SubscriptionsPushOwner = "SUBSCRIPTIONS_PUSH_OWNER";
        public const string SubscriptionsPushPublicKey = "SUBSCRIPTIONS_PUSH_PUBLIC_KEY";
        public const string SubscriptionsPushPrivateKey = "SUBSCRIPTIONS_PUSH_PRIVATE_KEY";
        public const string SubscriptionsSmtpServerAddressKey = "SUBSCRIPTIONS_SMTP_SERVER_ADDRESS";
        public const string SubscriptionsSmtpPortKey = "SUBSCRIPTIONS_SMTP_PORT";
        public const string SubscriptionsSmtpUseSslKey = "SUBSCRIPTIONS_SMTP_USE_SSL";
        public const string SubscriptionsSmtpSenderNameKey = "SUBSCRIPTIONS_SMTP_SENDER_NAME";
        public const string SubscriptionsSmtpSenderAddressKey = "SUBSCRIPTIONS_SMTP_SENDER_ADDRESS";
        public const string SubscriptionsSmtpSenderUsernameKey = "SUBSCRIPTIONS_SMTP_SENDER_USERNAME";
        public const string SubscriptionsSmtpSenderPasswordKey = "SUBSCRIPTIONS_SMTP_SENDER_PASSWORD";
        public const string SubscriptionsCortexGraphOutBaseUrl = "SUBSCRIPTIONS_CORTEX_GRAPH_OUT_BASE_URL";
    }
}