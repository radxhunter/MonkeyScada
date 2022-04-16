using CommunicationManager.Api.Helpers;
using CommunicationManager.Api.Services;
using FluentModbus;
using MonkeyScada.Shared.Redis;
using MonkeyScada.Shared.Serialization;
using MonkeyScada.Shared.Streaming;
using MonkeyScada.Shared.Redis.Streaming;
using System.Net;
using CommunicationManager.Api.Modbus.Services;
using CommunicationManager.Api.Modbus.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMeasurementService, MeasurementService>();
builder.Services.AddSingleton<IDeviceEnrollmentService, DeviceEnrollmentService>();

builder.Services
    .AddStreaming()
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddSingleton<ModbusRequestChannel>()
    .AddSingleton<IModbusCommunicator, ModbusReader>()
    .AddHostedService<ModbusBackgroundService>()
    .AddGrpc();

var app = builder.Build();
app.MapGrpcService<ModbusGrpcService>();

app.MapGet("/", () => "MonkeyScada CommunicationManager");

app.MapPost("/modbus/start", async (ModbusRequestChannel channel) =>
 {
     await channel.Requests.Writer.WriteAsync(new StartModbus());
     return Results.Ok();
 });

app.MapPost("/modbus/stop", async (ModbusRequestChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StopModbus());
    return Results.Ok();
});

app.MapControllers();

app.Run();
