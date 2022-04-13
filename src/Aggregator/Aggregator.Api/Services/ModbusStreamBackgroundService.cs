using Aggregator.Api.Models;
using MonkeyScada.Shared.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.Services
{
    public class ModbusStreamBackgroundService : BackgroundService
    {
        private readonly IStreamSubscriber _subscriber;
        private readonly ILogger<ModbusStreamBackgroundService> _logger;

        public ModbusStreamBackgroundService(IStreamSubscriber streamSubscriber, ILogger<ModbusStreamBackgroundService> logger)
        {
            _subscriber = streamSubscriber;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _subscriber.SubscribeAsync<MeasurementPair>("modbus", m =>
            {
                _logger.LogInformation($"The '{m.SensorName}' has value: '{m.Value}', timestamp: '{m.Timestamp}'");
            });
        }
    }
}
