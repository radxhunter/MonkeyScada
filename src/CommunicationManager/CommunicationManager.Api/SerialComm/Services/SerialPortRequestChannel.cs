using CommunicationManager.Api.Modbus.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CommunicationManager.Api.SerialComm.Services
{
    internal sealed class SerialPortRequestChannel
    {
        public readonly Channel<ISerialPortRequest> Requests = Channel.CreateUnbounded<ISerialPortRequest>();
    }
}
