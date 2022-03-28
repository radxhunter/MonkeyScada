using EasyModbus;
using ModbusClientConsole.Helpers;
using System;
namespace ModbusClientConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ModbusClient client = new ModbusClient("192.168.100.2", 502);
            client.ConnectionTimeout = 3000;
            client.Connect();
            Console.WriteLine($"Connected to modbus server: {client.Connected}");

            client.WriteMultipleRegisters(0, new int[] { 1, 2, 3 });
            client.ReadHoldingRegisters(0, 4);
            Console.ReadKey();
          
            // TODO: (5) Write Multiple Register 
            // TODO: (5) Read Multiple Register 
        }
    }
}
