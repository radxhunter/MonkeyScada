using CommunicationManager.Api.Modbus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Modbus.Services
{
    internal sealed class ModbusGenerator : IModbusCommunicator
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

        public event EventHandler<MeasurementPair<double, long>>? MeasurementUpdated;

        public IEnumerable<string> GetSensorNames() => _measurementPairs.Keys;


        public async IAsyncEnumerable<MeasurementPair<double, long>> StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            while (_isRunning)
            {
                foreach (var (room, temperature) in _measurementPairs)
                {
                    if (!_isRunning)
                    {
                        yield break;
                    }

                    var tick = NextTick();
                    var newMeasurement = temperature + tick;
                    _measurementPairs[room] = newMeasurement;

                    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    _logger.LogInformation($"Updated modbus for: {room}, {temperature:F} -> {newMeasurement:F} [{tick:F}]");
                    var measurementPair = new MeasurementPair<double, long>(room, newMeasurement, timestamp);
                    MeasurementUpdated?.Invoke(this, measurementPair);
                    yield return measurementPair;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
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
