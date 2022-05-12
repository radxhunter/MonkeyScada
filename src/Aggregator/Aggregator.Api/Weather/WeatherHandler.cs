using Aggregator.Api.Events;
using Aggregator.Api.Models;
using MonkeyScada.Shared.Messaging;

namespace Aggregator.Api.Weather
{
    internal sealed class WeatherHandler : IWeatherHandler
    {
        private int _counter;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<WeatherHandler> _logger;

        public WeatherHandler(IMessagePublisher messagePublisher, ILogger<WeatherHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(WeatherData weatherData)
        {
            // TODO: Implement some actual business logic
            if (IsValueToHigh())
            {
                var alarmId = Guid.NewGuid().ToString("N");
                _logger.LogWarning($"Alarm with ID: {alarmId} has been raised for temperature: " +
                    $"'{weatherData.Temperature}' in City: '{weatherData.Location}'.");
                var integrationEvent = new RaisedAlarm(alarmId, "Temperature: " + weatherData.Temperature);
                await _messagePublisher.PublishAsync("alarms", integrationEvent);
            }
        }

        private bool IsValueToHigh()
         => Interlocked.Increment(ref _counter) % 10 == 0;
    }
}
