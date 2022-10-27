using ei8.Cortex.Subscriptions.Common;
using System.Collections.Generic;

namespace ei8.Cortex.Subscriptions.Application.Notifications
{
    public class BaseTemplateApplicationService
    {
        private readonly ISettingsService settingsService;

        public BaseTemplateApplicationService(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        protected void SetDefaultAvatarUrlParameter(Dictionary<string, object> templateValues)
        {
            if (templateValues.TryGetValue(NotificationTemplateParameters.AvatarUrl, out _) == false)
                templateValues[NotificationTemplateParameters.AvatarUrl] = this.settingsService.AvatarUrl;
        }
    }
}
