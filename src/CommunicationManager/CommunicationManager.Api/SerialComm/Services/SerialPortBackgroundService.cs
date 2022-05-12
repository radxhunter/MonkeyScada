using CommunicationManager.Api.Modbus.Requests;
using MonkeyScada.Shared.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.SerialComm.Services
{
    internal class SerialPortBackgroundService : BackgroundService
    {
        private readonly ILogger<SerialPortBackgroundService> _logger;
        private readonly ISerialPortCommunicator _serialPortCommunicator;
        private readonly SerialPortRequestChannel _requestChannel;
        private int _runningStatus;

        public SerialPortBackgroundService(
            ISerialPortCommunicator serialPortCommunicator,
            SerialPortRequestChannel requestChannel,
            ILogger<SerialPortBackgroundService> logger)
        {
            _logger = logger;
            _serialPortCommunicator = serialPortCommunicator;
            _requestChannel = requestChannel;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _ = _serialPortCommunicator.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serialPortCommunicator.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serial background service has started");
            await foreach (var request in _requestChannel.Requests.Reader.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation($"Serial background service has received the request: '{ request.GetType().Name}'");
                var _ = request switch
                {
                    StartSerial => StartGeneratorAsync(stoppingToken),
                    StopSerial => StopGeneratorAsync(stoppingToken),
                    _ => Task.CompletedTask
                };
            }
            _logger.LogInformation("Serial background service has stopped");
        }

        private async Task StartGeneratorAsync(CancellationToken cancellationToken)
        {
            if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
            {
                _logger.LogInformation("Serial generator is already running");
                return;
            }

            await foreach (var m in _serialPortCommunicator.StartAsync(cancellationToken))
            {
                //_logger.LogInformation($"{m.SensorName}: {m.Value}, timestamp: {m.Timestamp}.");
                //_logger.LogInformation("Publishing the measurement pair...");
                //await _streamPublisher.PublishAsync("modbus", measurementPair);
            }
        }

        private async Task StopGeneratorAsync(CancellationToken cancellationToken)
        {
            if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
            {
                _logger.LogInformation("Serial generator is not running");
                return;
            }

            await _serialPortCommunicator.StopAsync(cancellationToken);
        }
    }
}
