using CommunicationManager.Api.Helpers;
using CommunicationManager.Api.Requests;
using CommunicationManager.Api.Services;
using FluentModbus;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMeasurementService, MeasurementService>();
builder.Services.AddSingleton<IDeviceEnrollmentService, DeviceEnrollmentService>();
builder.Services.AddSingleton<ModbusRequestChannel>()
    .AddSingleton<IModbusCommunicator, ModbusReader>()
    .AddHostedService<ModbusBackgroundService>();

var app = builder.Build();

//var client = new ModbusTcpClient();
//client.Connect(new IPEndPoint(IPAddress.Parse(IpLocalizer.GetLocalIpAddress()), 502), ModbusEndianness.BigEndian);

//await client.WriteMultipleRegistersAsync(0, 0, new short[] { 11, 201, 3001, 41 }, CancellationToken.None);
//var data = (await client.ReadHoldingRegistersAsync<short>(0,0,10, CancellationToken.None)).ToArray();
////client.WriteMultipleRegisters(0, 0, new short[] { 1, 20, 300, 4000 });
////var data = client.ReadHoldingRegisters<short>(0, 0, 10);
//Console.WriteLine($"{data[0]}, {data[1]}, {data[2]}");


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
