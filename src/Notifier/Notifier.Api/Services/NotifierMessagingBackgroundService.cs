using MonkeyScada.Shared.Messaging;
using Notifier.Api.Events.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.Api.Services
{
    internal sealed class NotifierMessagingBackgroundService : BackgroundService
    {
        private readonly IMessageSubscriber _messageSubscriber;
        private readonly ILogger<NotifierMessagingBackgroundService> _logger;

        public NotifierMessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<NotifierMessagingBackgroundService> logger)
        {
            _messageSubscriber = messageSubscriber;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageSubscriber.SubscribeAsync<RaisedAlarm>("alarms", message =>
            {
                _logger.LogInformation($"Alarm with ID: '{message.AlarmId}' for sensor name: '{message.SensorName}' has beed raised");
            });

            return Task.CompletedTask;
        }
    }
}
