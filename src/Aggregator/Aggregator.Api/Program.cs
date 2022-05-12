using Aggregator.Api.Modbus;
using Aggregator.Api.SerialComm;
using Aggregator.Api.Weather;
using MonkeyScada.Shared.Messaging;
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
    .AddHostedService<SerialStreamBackgroundService>()
    .AddHostedService<WeatherStreamBackgroundService>()
    .AddSerialization()
    .AddStreaming()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddMessaging()
    .AddSingleton<IModbusHandler, ModbusHandler>()
    .AddSingleton<ISerialHandler, SerialHandler>()
    .AddSingleton<IWeatherHandler, WeatherHandler>()
    ;

var app = builder.Build();

app.MapGet("/", () => "MonkeyScada Aggregator");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
