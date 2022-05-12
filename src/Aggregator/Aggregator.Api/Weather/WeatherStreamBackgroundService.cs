using Aggregator.Api.Models;
using MonkeyScada.Shared.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.Weather
{
    internal sealed class WeatherStreamBackgroundService : BackgroundService
    {
        private readonly IStreamSubscriber _streamSubscriber;
        private readonly IWeatherHandler _weatherHandler;
        private readonly ILogger<WeatherStreamBackgroundService> _logger;

        public WeatherStreamBackgroundService(IStreamSubscriber streamSubscriber,
            IWeatherHandler weatherHandler,
            ILogger<WeatherStreamBackgroundService> logger)
        {
            _streamSubscriber = streamSubscriber;
            _weatherHandler = weatherHandler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _streamSubscriber.SubscribeAsync<WeatherData>("weather", data =>
            {
                _logger.LogInformation($"{data.Location}: {data.Temperature} 'C, {data.Humidity} %," +
                    $"{data.Wind} km/h [{data.Condition}]");
                _ = _weatherHandler.HandleAsync(data);
            });
        }
    }
}
