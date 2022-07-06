using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Domain.Model;

namespace ei8.Cortex.Subscriptions.In.Api.BackgroundServices
{
    public class AvatarPollingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider services;

        public AvatarPollingBackgroundService(IServiceProvider services)
        {
            this.services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = this.services.CreateScope())
                    {
                        var pollingService = scope.ServiceProvider.GetService<IPollingApplicationService>();
                        var subscriptionService = scope.ServiceProvider.GetService<ISubscriptionApplicationService>();
                        var settings = scope.ServiceProvider.GetService<ISettingsService>();
                        var logger = scope.ServiceProvider.GetService<ILogger<AvatarPollingBackgroundService>>();

                        try
                        {
                            var avatars = await pollingService.GetAvatarsForPollingAsync();

                            foreach (var a in avatars)
                                await this.PollAvatarAsync(a, pollingService, subscriptionService, logger);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Error running polling service {Message}", ex.Message);
                            continue;
                        }

                        await Task.Delay(TimeSpan.FromSeconds(settings.PollingIntervalSeconds));
                    }
                }
            });
        }

        private async Task PollAvatarAsync(Avatar a, 
            IPollingApplicationService pollingService, 
            ISubscriptionApplicationService subscriptionService,
            ILogger logger)
        {
            try
            {
                logger.LogInformation($"Begin polling {a.Url}.");

                var hasChanges = await pollingService.CheckForChangesAsync(a);

                if (hasChanges)
                    await subscriptionService.NotifySubscribers(a);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error polling avatar {a.Url}: {Message}", a.Url, ex.Message);
            }
        }
    }
}
