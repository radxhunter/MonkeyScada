using Aggregator.Api.Models;
using MonkeyScada.Shared.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.Services
{
    internal sealed class ModbusStreamBackgroundService : BackgroundService
    {
        private readonly IStreamSubscriber _subscriber;
        private readonly IModbusHandler _modbusHandler;
        private readonly ILogger<ModbusStreamBackgroundService> _logger;

        public ModbusStreamBackgroundService(IStreamSubscriber streamSubscriber,
            IModbusHandler modbusHandler,
            ILogger<ModbusStreamBackgroundService> logger)
        {
            _subscriber = streamSubscriber;
            _modbusHandler = modbusHandler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _subscriber.SubscribeAsync<MeasurementPair>("modbus", m =>
            {
                _logger.LogInformation($"The '{m.SensorName}' has value: '{m.Value}', timestamp: '{m.Timestamp}'");
                _ = _modbusHandler.HandleAsync(m);
            });
        }
    }
}
