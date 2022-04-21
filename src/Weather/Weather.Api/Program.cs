using MonkeyScada.Shared.HTTP;
using MonkeyScada.Shared.Redis;
using MonkeyScada.Shared.Redis.Streaming;
using MonkeyScada.Shared.Serialization;
using MonkeyScada.Shared.Streaming;
using Weather.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSerialization()
    .AddStreaming()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddHostedService<WeatherBackgroundService>()
    .AddHttpClient()
    .AddHttpApiClient<IWeatherFeed, WeatherFeed>();

var app = builder.Build();

app.MapGet("/", () => "MonkeyScada Weather");

app.UseSwagger();
app.UseSwaggerUI();


app.Run();
