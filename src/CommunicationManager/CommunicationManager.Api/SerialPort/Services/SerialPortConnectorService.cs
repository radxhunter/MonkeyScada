using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.SerialPortConnector.Services
{
    // TODO: Create background services, controlled by api methods: /start and /stop
    public class SerialPortConnectorService : ISerialPortConnectorService
    {
        private SerialPort _serialPort;
        private readonly string _portName;
        private readonly int _baudRate;

        private readonly ILogger<SerialPortConnectorService> _logger;

        public SerialPortConnectorService(IConfiguration configuration, ILogger<SerialPortConnectorService> logger)
        {
            _portName = configuration.GetSection("SerialCommunication").GetValue<string>("PortName");
            _baudRate = configuration.GetSection("SerialCommunication").GetValue<int>("BaudRate");
            _logger = logger;
            _serialPort = new SerialPort(_portName, _baudRate);
        }

        public void Read()
        {
            // Attach a method to be called when there is data waiting in the port's buffer
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            _serialPort.Open();
        }

        public void Send(string command)
        {
            using var serialPort = new SerialPort(_portName, _baudRate);
            serialPort.Open();
            serialPort.Write(command);
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _logger.LogInformation($"Data received: {_serialPort.ReadExisting()}");
            Thread.Sleep(1000);
        }
    }
}
