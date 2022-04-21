using Aggregator.Api.Services;
using MonkeyScada.Shared.Redis;
using MonkeyScada.Shared.Redis.Streaming;
using MonkeyScada.Shared.Serialization;
using MonkeyScada.Shared.Streaming;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHostedService<ModbusStreamBackgroundService>()
    .AddHostedService<WeatherStreamBackgroundService>()
    .AddSerialization()
    .AddStreaming()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming();

var app = builder.Build();

app.MapGet("/", () => "MonkeyScada Aggregator");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
