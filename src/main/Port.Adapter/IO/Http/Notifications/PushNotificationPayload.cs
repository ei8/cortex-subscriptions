using System;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.IO.Http.Notifications
{
    public class PushNotificationPayload
    {
        public string Title { get; set; }

        public Dictionary<string, object> Options { get; set; } = new Dictionary<string, object>();

        public string Body
        {
            get => GetOptionValueOrNull<string>(nameof(Body));
            set => AddOrUpdateOption(nameof(Body), value);
        }

        public string Tag
        {
            get => GetOptionValueOrNull<string>(nameof(Tag));
            set => AddOrUpdateOption(nameof(Tag), value);
        }

        public string Icon
        {
            get => GetOptionValueOrNull<string>(nameof(Icon)) ?? String.Empty;
            set => AddOrUpdateOption(nameof(Icon), value);
        }

        public string Badge
        {
            get => GetOptionValueOrNull<string>(nameof(Badge));
            set => AddOrUpdateOption(nameof(Badge), value);
        }

        public List<PushNotificationAction> Actions
        {
            get => GetOptionValueOrNull<List<PushNotificationAction>>(nameof(Actions));
            set => AddOrUpdateOption(nameof(Actions), value);
        }

        private T GetOptionValueOrNull<T>(string propName) where T : class
        {
            T value = null;

            if (Options != null)
            {
                object objectValue;
                Options.TryGetValue(propName.ToLower(), out objectValue);

                value = (T)objectValue;
            }

            return value;
        }

        private void AddOrUpdateOption(string propName, object value)
        {
            var actualPropName = propName.ToLower();

            if (Options?.ContainsKey(actualPropName) == true)
                Options[actualPropName] = value;

            else
                Options.Add(actualPropName, value);
        }
    }

    public class PushNotificationAction
    {
        public string Action { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
