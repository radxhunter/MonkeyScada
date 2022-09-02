using CommunicationManager.Api.Modbus.Requests;
using MonkeyScada.Shared.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Modbus.Services
{
    internal class ModbusBackgroundService : BackgroundService
    {
        private int _runningStatus;
        private readonly IModbusCommunicator _modbusCommunicator;
        private readonly ModbusRequestChannel _requestChannel;
        private readonly IStreamPublisher _streamPublisher;
        private readonly ILogger<ModbusBackgroundService> _logger;

        public ModbusBackgroundService(
            IModbusCommunicator modbusCommunicator, 
            ModbusRequestChannel channel,
            IStreamPublisher streamPublisher,
            ILogger<ModbusBackgroundService> logger)
        {
            _modbusCommunicator = modbusCommunicator;
            _requestChannel = channel;
            _streamPublisher = streamPublisher;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _ = _modbusCommunicator.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _modbusCommunicator.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Modbus background service has started");
            await foreach (var request in _requestChannel.Requests.Reader.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation($"Modbus background service has received the request: '{ request.GetType().Name}'");
                var _ = request switch
                {
                    StartModbus => StartModbusAsync(stoppingToken),
                    StopModbus => StopModbusAsync(stoppingToken),
                    _ => Task.CompletedTask
                };
            }
            _logger.LogInformation("Modbus background service has stopped");
        }

private async Task StartModbusAsync(CancellationToken cancellationToken)
{
    if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
    {
        _logger.LogInformation("Modbus is already running");
        return;
    }

    await foreach (var measurementPair in _modbusCommunicator.StartAsync(cancellationToken))
    {
        _logger.LogInformation("Publishing the measurement pair...");
        await _streamPublisher.PublishAsync("modbus", measurementPair);
    }
}

private async Task StopModbusAsync(CancellationToken cancellationToken)
{
    if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
    {
        _logger.LogInformation("Modbus is not running");
        return;
    }

    await _modbusCommunicator.StopAsync(cancellationToken);
}
    }
}
