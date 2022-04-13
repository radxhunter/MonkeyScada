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

        public async IAsyncEnumerable<MeasurementPair> StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            while (_isRunning)
            {
                short[] modbusData = new short[] { };
                long timestamp = 0;
                try
                {
                    ModbusTcpClient client = ConnectToLocalModbusServer();

                    modbusData = (await client.ReadHoldingRegistersAsync<short>(0, 0, 10, cancellationToken)).Span.Slice(0, 4).ToArray();
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    _logger.LogInformation($"Updated modbus: {modbusData[0]}, {modbusData[1]}, {modbusData[2]}, {modbusData[3]}");

                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occured: " + ex.Message, ex);
                }
                
                for (int i = 0; i < modbusData.Length; i++)
                {
                    //measurementPairs.Add(new MeasurementPair($"Register{i}", modbusData[i], timestamp));
                    yield return new MeasurementPair($"Register{i}", modbusData[i], timestamp);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
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
