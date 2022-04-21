using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.SerialPortConnector.Services
{
    public class SerialPortConnectorService : ISerialPortConnectorService
    {
        private readonly int _baudRate = 9600;
        private readonly string _portName = "COM3";

        public void Send(string command)
        {
            using var serialPort = new SerialPort(_portName, _baudRate);
            serialPort.Open();
            serialPort.Write(command);
        }
    }
}
