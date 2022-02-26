using FluentModbus;
using ModbusClientConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModbusClientConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ModbusTcpClient client = new ModbusTcpClient();
            int unitIdentifier = 0x01;

            client.Connect(new IPEndPoint(IPAddress.Parse(IpLocalizer.GetLocalIpAddress()), 502), ModbusEndianness.BigEndian);

            client.WriteSingleRegister(unitIdentifier, 2, 3);

            // TODO: (5) Write Multiple Register https://apollo3zehn.github.io/FluentModbus/
            // TODO: (5) Read Multiple Register https://apollo3zehn.github.io/FluentModbus/
        }
    }
}
