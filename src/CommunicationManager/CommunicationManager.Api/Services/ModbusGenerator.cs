using CommunicationManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Services
{
    internal sealed class ModbusGenerator : IModbusGenerator
    {
        private readonly ILogger<ModbusGenerator> _logger;
        private readonly Random _random = new Random();
        private readonly Dictionary<string, double> _measurementPairs = new()
        {
            ["LivingRoomTemperature"] = 20.5,
            ["BathRoomTemperature"] = 22.5,
            ["BedRoomTemperature"] = 20
        };

        private bool _isRunning;

        public ModbusGenerator(ILogger<ModbusGenerator> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _isRunning = true;
            while (_isRunning)
            {
                foreach (var (room, temperature) in _measurementPairs)
                {
                    if (!_isRunning)
                    {
                        return;
                    }

                    var tick = NextTick();
                    var newMeasurement = temperature + tick;
                    _measurementPairs[room] = newMeasurement;

                    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    _logger.LogInformation($"Updated pricing for: {room}, {temperature:F} -> {newMeasurement:F} [{tick:F}]");
                    var measurementPair = new MeasurementPair(room, newMeasurement, timestamp);

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        public Task StopAsync()
        {
            _isRunning = false;
            return Task.CompletedTask;
        }

        private double NextTick()
        {
            var sign = _random.Next(0, 2) == 0 ? -1 : 1;
            var tick = _random.NextDouble() / 20;
            return sign * tick;
        }
    }
}
