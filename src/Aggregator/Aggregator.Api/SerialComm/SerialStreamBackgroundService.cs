using Aggregator.Api.Models;
using MonkeyScada.Shared.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.SerialComm
{
    internal sealed class SerialStreamBackgroundService : BackgroundService
    {
        private readonly IStreamSubscriber _subscriber;
        private readonly ISerialHandler _serialHandler;
        private readonly ILogger<SerialStreamBackgroundService> _logger;

        public SerialStreamBackgroundService(IStreamSubscriber streamSubscriber,
            ISerialHandler serialHandler,
            ILogger<SerialStreamBackgroundService> logger)
        {
            _subscriber = streamSubscriber;
            _serialHandler = serialHandler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _subscriber.SubscribeAsync<MeasurementPair<string, DateTime>>("serial", m =>
            {
                _logger.LogInformation($"The '{m.SensorName}' has value: '{m.Value}', time: '{m.Time}'");
                _ = _serialHandler.HandleAsync(m);
            });
        }
    }
}
