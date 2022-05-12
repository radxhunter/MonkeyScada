using CommunicationManager.Api.Modbus.Models;
using MediatR;
using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommunicationManager.Api.SerialComm.Services
{
    public class SerialPortConnectorService : ISerialPortConnectorService, ISerialPortCommunicator
    {
        private readonly string _portName;
        private readonly int _baudRate;
        private bool _isRunning;
        private readonly ILogger<SerialPortConnectorService> _logger;

        public event EventHandler<MeasurementPair<string, DateTime>>? MeasurementUpdated;

        public SerialPortConnectorService(IConfiguration configuration, ILogger<SerialPortConnectorService> logger)
        {
            _portName = configuration.GetSection("SerialCommunication").GetValue<string>("PortName");
            _baudRate = configuration.GetSection("SerialCommunication").GetValue<int>("BaudRate");
            _logger = logger;
        }

        private Queue<MeasurementPair<string, DateTime>> measurements = new();

        public void Send(string command)
        {
            using var serialPort = new SerialPort(_portName, _baudRate);
            serialPort.Open();
            serialPort.Write(command);
        }

        public async IAsyncEnumerable<MeasurementPair<string, DateTime>> StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            using var serialPort = new SerialPortStream(_portName, _baudRate);
            while (_isRunning)
            {
                await ReadSerial(serialPort, cancellationToken);

                foreach (var measurement in measurements)
                {
                    MeasurementUpdated?.Invoke(this, measurement);
                    yield return measurement;
                }

                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }

        private Task ReadSerial(SerialPortStream serialPort, CancellationToken cancellationToken)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                if (serialPort.BytesToRead > 0)
                {
                    string serialData = serialPort.ReadLine();
                    var time = DateTime.Now;
                    _logger.LogInformation("Data Received:------->" + serialData + ", time:" + time);

                    measurements.Enqueue(new MeasurementPair<string, DateTime>(_portName, serialData, time));
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured: " + ex.Message, ex);
                throw;
            }
            return Task.CompletedTask;
        }
    }
}

