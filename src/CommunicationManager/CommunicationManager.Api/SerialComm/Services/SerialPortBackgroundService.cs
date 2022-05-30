using CommunicationManager.Api.SerialComm.Requests;
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
        private readonly ISerialPortReceiver _serialPortCommunicator;
        private readonly SerialPortRequestChannel _requestChannel;
        private readonly IStreamPublisher _streamPublisher;
        private int _runningStatus;

        public SerialPortBackgroundService(
            ISerialPortReceiver serialPortCommunicator,
            SerialPortRequestChannel requestChannel,
            IStreamPublisher streamPublisher,
            ILogger<SerialPortBackgroundService> logger)
        {
            _logger = logger;
            _serialPortCommunicator = serialPortCommunicator;
            _requestChannel = requestChannel;
            _streamPublisher = streamPublisher;
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

            await foreach (var measurementPair in _serialPortCommunicator.StartAsync(cancellationToken))
            {
                await _streamPublisher.PublishAsync("serial", measurementPair);
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
