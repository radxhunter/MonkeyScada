using EasyModbus;
using ModbusClientConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServerConsole
{
    public delegate void Notify();

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running modbus server...");
            ModbusServer modbusServer = new ModbusServer();
            modbusServer.Listen();
            Console.WriteLine($"Modbus Server is running and has IP Address: { IpLocalizer.GetLocalIPAddress() }... UnitIdentifier is { modbusServer.UnitIdentifier } \r\n Type something to break");
            //TODO: (5) Create logging functionality: Add infinite loop that handle the event when someone connect to the server, read/write some data etc.; 

            Console.ReadKey();
        }
    }
}