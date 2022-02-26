using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running modbus server...");
            ModbusServer modbusServer = new ModbusServer();
            modbusServer.Listen();
            //TODO: (1) Add event handling when someone connect to server, write some data etc.
            Console.WriteLine($"Modbus Server running on IP { modbusServer.LocalIPAddress } or { IpLocalizer.GetLocalIPAddress() }... UnitIdentifier is { modbusServer.UnitIdentifier } \r\n Type something to break");
            Console.ReadKey();
        }
    }
}
