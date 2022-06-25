using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Application.Interface.Repository;
using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.In.Api.Settings;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Persistence.SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ISettingsService, EnvironmentSettingsService>();
builder.Services.AddTransient<IAvatarRepository, AvatarRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IBrowserReceiverRepository, BrowserReceiverRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapPost("/subscriptions", async (BrowserSubscriptionInfo request, ISubscriptionService service) =>
{
    await service.AddSubscriptionForBrowserAsync(request);
    return Results.Ok();
});

// TODO: remove later. for debugging only
app.MapGet("/subscriptions/{userNeuronId}", async (Guid userNeuronId, ISubscriptionService service) =>
{
    var subs = await service.GetSubscriptionsForUserAsync(userNeuronId);
    return Results.Ok(subs);
});

app.Run();