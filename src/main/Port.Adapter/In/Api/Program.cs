using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Domain.Model;
using ei8.Cortex.Subscriptions.In.Api.BackgroundServices;
using ei8.Cortex.Subscriptions.In.Api.Settings;
using ei8.Cortex.Subscriptions.IO.Http.Notifications;
using ei8.Cortex.Subscriptions.IO.Http.PayloadHashing;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ISettingsService, EnvironmentSettingsService>();
builder.Services.AddTransient<IAvatarRepository, AvatarRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IBrowserReceiverRepository, BrowserReceiverRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddTransient<ISubscriptionApplicationService, SubscriptionApplicationService>();
builder.Services.AddTransient<IPollingApplicationService, PollingApplicationService>();
builder.Services.AddTransient<IPayloadHashService, HttpPayloadHashService>();
builder.Services.AddTransient<IPushNotificationService, PushNotificationService>();
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

app.MapPost("/subscriptions", async (BrowserSubscriptionInfo request, ISubscriptionApplicationService service) =>
{
    await service.AddSubscriptionForBrowserAsync(request);
    return Results.Ok();
});

// TODO: remove later. for debugging only
app.MapGet("/subscriptions/{userNeuronId}", async (Guid userNeuronId, ISubscriptionApplicationService service) =>
{
    var subs = await service.GetAllByUserIdAsync(userNeuronId);
    return Results.Ok(subs);
});

app.Run();