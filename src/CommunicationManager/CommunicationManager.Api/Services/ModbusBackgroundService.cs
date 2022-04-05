using CommunicationManager.Api.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Services
{
    internal class ModbusBackgroundService : BackgroundService
    {
        private int _runningStatus;
        private readonly IModbusGenerator _modbusGenerator;
        private readonly ModbusRequestChannel _requestChannel;
        private readonly ILogger<ModbusBackgroundService> _logger;

        public ModbusBackgroundService(IModbusGenerator modbusGenerator, ModbusRequestChannel channel,
            ILogger<ModbusBackgroundService> logger)
        {
            _modbusGenerator = modbusGenerator;
            _requestChannel = channel;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _ = _modbusGenerator.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _modbusGenerator.StopAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Modbus background service has started");
            await foreach (var request in _requestChannel.Requests.Reader.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation($"Modbus background service has received the request: '{ request.GetType().Name}'");
                var _ = request switch
                {
                    StartModbus => StartGeneratorAsync(),
                    StopModbus => StopGeneratorAsync(),
                    _ => Task.CompletedTask
                };
            }
            _logger.LogInformation("Modbus background service has stopped");
        }

        private async Task StartGeneratorAsync()
        {
            if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
            {
                _logger.LogInformation("Modbus generator is already running");
                return;
            }
            await _modbusGenerator.StartAsync();
        }

        private async Task StopGeneratorAsync()
        {
            if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
            {
                _logger.LogInformation("Modbus generator is not running");
                return;
            }

            await _modbusGenerator.StopAsync();
        }
    }
}
