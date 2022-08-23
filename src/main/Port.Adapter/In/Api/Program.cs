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
builder.Services.AddScoped<ISettingsService, EnvironmentSettingsService>();
builder.Services.AddScoped<IAvatarUrlSnapshotRepository, AvatarUrlSnapshotRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrowserReceiverRepository, BrowserReceiverRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionApplicationService, SubscriptionApplicationService>();
builder.Services.AddScoped<IPollingApplicationService, PollingApplicationService>();
builder.Services.AddScoped<IPushNotificationApplicationService, WebPushNotificationApplicationService>();
builder.Services.AddScoped<PushNotificationSettings>(sp =>
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

app.Run();