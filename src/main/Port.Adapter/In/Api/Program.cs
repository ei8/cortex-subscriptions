using ei8.Cortex.Graph.Client;
using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Application.Interface.Service.PushNotifications;
using ei8.Cortex.Subscriptions.Application.Notifications;
using ei8.Cortex.Subscriptions.Application.PushNotifications;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.In.Api.BackgroundServices;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Process.Services;
using ei8.Net.Email;
using ei8.Net.Email.Smtp.Notifications;
using ei8.Net.Http;
using ei8.Net.Http.Notifications;
using Microsoft.AspNetCore.Mvc;
using neurUL.Common.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISettingsService, EnvironmentSettingsService>();
builder.Services.AddScoped<IAvatarUrlSnapshotRepository, AvatarUrlSnapshotRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrowserReceiverRepository, BrowserReceiverRepository>();
builder.Services.AddScoped<ISmtpReceiverRepository, SmtpReceiverRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionApplicationService, SubscriptionApplicationService>();
builder.Services.AddScoped<IPollingApplicationService, PollingApplicationService>();
builder.Services.AddScoped<INotificationApplicationService, WebPushNotificationApplicationService>();
builder.Services.AddScoped<INotificationApplicationService, SmtpNotificationApplicationService>();
builder.Services.AddScoped<IRequestProvider>(sp =>
                {
                    var rp = new RequestProvider();
                    rp.SetHttpClientHandler(new HttpClientHandler());
                    return rp;
                });
builder.Services.AddScoped<INeuronGraphQueryClient, HttpNeuronGraphQueryClient>();
builder.Services.AddScoped<INotificationTemplateApplicationService<WebPushNotificationPayload>, WebPushTemplateApplicationService>();
builder.Services.AddScoped<INotificationTemplateApplicationService<SmtpNotificationPayload>, SmtpNotificationTemplateApplicationService>();

builder.Services.AddScoped<WebPushNotificationSettings>(sp =>
{
    // inject push notification settings from environment variables
    // through the main settings object
    var settings = sp.GetService<ISettingsService>();
    return settings.PushSettings;
});
builder.Services.AddScoped<SmtpNotificationSettings>(sp =>
{
    // inject notification settings from environment variables
    // through the main settings object
    var settings = sp.GetService<ISettingsService>();
    return settings.SmtpSettings;
});

builder.Services.AddHttpServices();
builder.Services.AddEmailServices();
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
    var requestResult = await neurUL.Common.Api.Helper.ProcessRequestCore(
        null,
        async() =>
        {
            switch (receiverType)
            {
                case "web":
                    var obj = await request.ReadFromJsonAsync<AddSubscriptionWebReceiverRequest>();
                    await service.AddSubscriptionAsync(obj.SubscriptionInfo, obj.ReceiverInfo);
                    break;
                case "smtp":
                    var obj2 = await request.ReadFromJsonAsync<AddSubscriptionSmtpReceiverRequest>();
                    await service.AddSubscriptionAsync(obj2.SubscriptionInfo, obj2.ReceiverInfo);
                    break;
            }
        }
        );

    return requestResult == null ? Results.Ok() : Results.Problem(requestResult.ToString());
});

app.MapPost("/notify/{targetUserNeuronId}", async (Guid targetUserNeuronId, 
    [FromBody] NotificationPayloadRequest payload, 
    IEnumerable<INotificationApplicationService> pushNotificationApplicationServices) =>
{
    foreach (var service in pushNotificationApplicationServices)
    {
        await service.NotifyReceiversForUserAsync(targetUserNeuronId, payload.TemplateType, payload.TemplateValues);
    }
});

app.Run();