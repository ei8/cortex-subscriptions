using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Common;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Process.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISettingsService, EnvironmentSettingsService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapGet("/config", (ISettingsService settings) =>
{
    return new SubscriptionConfiguration
    {
        ServerPublicKey = settings.PushSettings.PushPublicKey
    };
});

app.Run();