using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications;
using ei8.Cortex.Subscriptions.Application.PushNotifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.In.Api.BackgroundServices;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Process.Services;
using ei8.Net.Http;
using ei8.Net.Http.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ISettingsService, EnvironmentSettingsService>();
builder.Services.AddTransient<IAvatarUrlSnapshotRepository, AvatarUrlSnapshotRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IBrowserReceiverRepository, BrowserReceiverRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddTransient<ISubscriptionApplicationService, SubscriptionApplicationService>();
builder.Services.AddTransient<IPollingApplicationService, PollingApplicationService>();
builder.Services.AddTransient<IPushNotificationApplicationService, WebPushNotificationApplicationService>();
builder.Services.AddTransient<PushNotificationSettings>(sp =>
{
    // inject push notification settings from environment variables
    // through the main settings object
    var settings = sp.GetService<ISettingsService>();
    return settings.PushSettings;
});

builder.Services.AddEi8Http();

builder.Services.AddHttpClient();

// add swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add background services
builder.Services.AddHostedService<AvatarPollingBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapPost("/subscriptions/receivers/{receiverType}", async (string receiverType, HttpRequest request, ISubscriptionApplicationService service) =>
{
    switch (receiverType)
    {
        case "web":
            var obj = await request.ReadFromJsonAsync<AddSubscriptionWebReceiverRequest>();
            await service.AddSubscriptionAsync(obj.SubscriptionInfo, obj.ReceiverInfo);
            break;
    }

    return Results.Ok();
});

// TODO: remove later. for debugging only
app.MapGet("/subscriptions/{userNeuronId}", async (Guid userNeuronId, ISubscriptionApplicationService service) =>
{
    var subs = await service.GetAllByUserIdAsync(userNeuronId);
    return Results.Ok(subs);
});

app.Run();