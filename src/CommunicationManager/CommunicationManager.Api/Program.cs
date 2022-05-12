using CommunicationManager.Api.Services;
using MonkeyScada.Shared.Redis;
using MonkeyScada.Shared.Serialization;
using MonkeyScada.Shared.Streaming;
using MonkeyScada.Shared.Redis.Streaming;
using CommunicationManager.Api.Modbus.Services;
using CommunicationManager.Api.Modbus.Requests;
using CommunicationManager.Api.SerialComm.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMeasurementService, MeasurementService>();
builder.Services.AddSingleton<IDeviceEnrollmentService, DeviceEnrollmentService>();

builder.Services
    .AddMediatR(typeof(Program))
    .AddStreaming()
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddSingleton<ModbusRequestChannel>()
    .AddSingleton<SerialPortRequestChannel>()
    .AddSingleton<IModbusCommunicator, ModbusReader>()
    .AddScoped<ISerialPortCommunicator, SerialPortConnectorService>()
    .AddScoped<ISerialPortConnectorService, SerialPortConnectorService>()
    .AddHostedService<ModbusBackgroundService>()
    .AddHostedService<SerialPortBackgroundService>()
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

app.MapPost("/serial/start", async (SerialPortRequestChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StartSerial());
    return Results.Ok();
});

app.MapPost("/serial/stop", async (SerialPortRequestChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StopSerial());
    return Results.Ok();
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
