using MonkeyScada.Shared.Messaging;
using MonkeyScada.Shared.Serialization;
using Notifier.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSerialization()
    .AddMessaging()
    .AddHostedService<NotifierMessagingBackgroundService>()
    ;

var app = builder.Build();

app.MapGet("/", () => "MonkeyScada Notifier");

app.Run();
