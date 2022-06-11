using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Application.Interface;
using ei8.Cortex.Subscriptions.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IAvatarRepository, AvatarRepositoryMock>();
builder.Services.AddSingleton<IUserRepository, UserRepositoryMock>();
builder.Services.AddSingleton<ISubscriptionRepository, SubscriptionRepositoryMock>();
builder.Services.AddSingleton<IBrowserReceiverRepository, BrowserReceiverRepositoryMock>();
builder.Services.AddSingleton<ISubscriptionService, SubscriptionService>();

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

app.MapPost("/subscriptions/add", async (BrowserSubscriptionInfo request, ISubscriptionService service) =>
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