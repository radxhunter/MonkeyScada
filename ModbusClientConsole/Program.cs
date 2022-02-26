using FluentModbus;
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
            string localIpAddress = Helpers.IpLocalizer.GetLocalIpAddress();
            int unitIdentifier = 0x01;

            client.Connect(new IPEndPoint(IPAddress.Parse(localIpAddress), 502), ModbusEndianness.BigEndian);

            client.WriteSingleRegister(unitIdentifier,5, 41);

            // TODO: (5) Write Multiple Register https://apollo3zehn.github.io/FluentModbus/
            // TODO: (5) Read Multiple Register https://apollo3zehn.github.io/FluentModbus/
        }
    }
}
