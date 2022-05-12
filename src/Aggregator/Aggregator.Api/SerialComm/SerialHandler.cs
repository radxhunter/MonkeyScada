using Aggregator.Api.Events;
using Aggregator.Api.Models;
using MonkeyScada.Shared.Messaging;

namespace Aggregator.Api.SerialComm
{
    internal sealed class SerialHandler : ISerialHandler
    {
        private int _counter;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<SerialHandler> _logger;

        public SerialHandler(IMessagePublisher messagePublisher, ILogger<SerialHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(MeasurementPair<string, DateTime> measurementPair)
        {
            // TODO: Implement some actual business logic
            if (IsValueToHigh())
            {
                var alarmId = Guid.NewGuid().ToString("N");
                _logger.LogWarning($"Alarm with ID: {alarmId} has been raised for sensor: " +
                    $"'{measurementPair.SensorName}' which has value: '{measurementPair.Value}'.");
                var integrationEvent = new RaisedAlarm(alarmId, measurementPair.SensorName);
                await _messagePublisher.PublishAsync("alarms", integrationEvent);
            }
        }

        private bool IsValueToHigh()
            => Interlocked.Increment(ref _counter) % 10 == 0;
    }
}
