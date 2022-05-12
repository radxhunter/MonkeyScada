using CommunicationManager.Api.Modbus.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Modbus.Services
{
    internal sealed class ModbusRequestChannel
    {
        public readonly Channel<IModbusRequest> Requests = Channel.CreateUnbounded<IModbusRequest>();
    }
}
