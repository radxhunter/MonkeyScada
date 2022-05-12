using CommunicationManager.Api.Helpers;
using CommunicationManager.Api.Modbus.Models;
using FluentModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Modbus.Services
{
    public class ModbusReader : IModbusCommunicator
    {
        private readonly ILogger<ModbusReader> _logger;
        private bool _isRunning;
        private Dictionary<string,short?> _modbusRegisters = new();

        public event EventHandler<MeasurementPair<double>>? MeasurementUpdated;

        public ModbusReader(ILogger<ModbusReader> logger)
        {
            _logger = logger;
            for (int i = 1; i < 5; i++)
            {
                _modbusRegisters.Add($"Register{i}", null);
            }
        }

        public IEnumerable<string> GetSensorNames() => _modbusRegisters.Keys;

        public async IAsyncEnumerable<MeasurementPair<double>> StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            while (_isRunning)
            {
                short[] modbusData = new short[] { };
                long timestamp = 0;
                try
                {
                    ModbusTcpClient client = ConnectToLocalModbusServer();

                    modbusData = (await client.ReadHoldingRegistersAsync<short>(0, 0, _modbusRegisters.Count, cancellationToken)).ToArray();
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    LogMessage("Modbus values: ", modbusData);
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occured: " + ex.Message, ex);
                }

                for (int i = 0; i < _modbusRegisters.Count; i++)
                {
                    var measurement = new MeasurementPair<double>(_modbusRegisters.Keys.ElementAt(i), modbusData[i], timestamp);
                    MeasurementUpdated?.Invoke(this, measurement);
                    yield return measurement;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private void LogMessage(string logMessage, short[] modbusData)
        {
            foreach (short data in modbusData)
            {
                logMessage += $"{data}, ";
            }
            _logger.LogInformation(logMessage.Remove(logMessage.Length - 1));
        }

        private static ModbusTcpClient ConnectToLocalModbusServer()
        {
            var client = new ModbusTcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse(IpLocalizer.GetLocalIpAddress()), 502), ModbusEndianness.BigEndian);
            return client;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }
    }
}
