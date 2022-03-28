using EasyModbus;
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
            ModbusClient client = new ModbusClient(IpLocalizer.GetLocalIpAddress(), 502);
            client.Connect();
            Console.WriteLine($"Connected to modbus server: {client.Connected}");

            Console.ReadKey();

            // TODO: (5) Write Multiple Register 
            // TODO: (5) Read Multiple Register 
        }
    }
}
