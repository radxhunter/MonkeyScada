using CommunicationManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMeasurementService, MeasurementService>();
builder.Services.AddSingleton<IDeviceEnrollmentService, DeviceEnrollmentService>();

var app = builder.Build();

app.MapGet("/", () => "MonkeyScada CommunicationManager");

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
