using Aggregator.Api.Services;
using MonkeyScada.Shared.Redis;
using MonkeyScada.Shared.Redis.Streaming;
using MonkeyScada.Shared.Serialization;
using MonkeyScada.Shared.Streaming;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHostedService<ModbusStreamBackgroundService>()
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddStreaming()
    .AddRedisStreaming()
    .AddRedis(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "MonkeyScada Aggregator");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
