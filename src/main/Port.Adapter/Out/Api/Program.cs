using ei8.Cortex.Subscriptions.Application;
using ei8.Cortex.Subscriptions.Application.Interface.Service;
using ei8.Cortex.Subscriptions.Port.Adapter.IO.Process.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISettingsService, EnvironmentSettingsService>();
builder.Services.AddTransient<ISubscriptionQueryService, SubscriptionQueryService>();

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

app.MapGet("/config", async (ISubscriptionQueryService subscriptionQueryService) =>
{
    return await subscriptionQueryService.GetServerConfigurationAsync();
});

app.Run();