using CommunicationManager.Api.Helpers;
using CommunicationManager.Api.Models;
using FluentModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Services
{
    public class ModbusReader : IModbusCommunicator
    {
        private readonly ILogger<ModbusReader> _logger;
        private bool _isRunning;

        public ModbusReader(ILogger<ModbusReader> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            while (_isRunning)
            {
                var client = new ModbusTcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse(IpLocalizer.GetLocalIpAddress()), 502), ModbusEndianness.BigEndian);

                var results = (await client.ReadHoldingRegistersAsync<short>(0, 0, 10, cancellationToken)).Span.Slice(0,4).ToArray();
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _logger.LogInformation($"Updated modbus: {results[0]}, {results[1]}, {results[2]}, {results[3]}");
                List<MeasurementPair> measurementPairs = new List<MeasurementPair>();
                int i = 0;
                foreach (var measurement in results)
                {
                    measurementPairs.Add(new MeasurementPair($"Register{i}", measurement, timestamp));
                    i++;
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }
    }
}
