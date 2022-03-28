using EasyModbus;
using ModbusClientConsole.Helpers;
using System;

namespace ModbusServerConsole
{
    public delegate void Notify();

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running modbus server...");
            ModbusClient client = new ModbusClient();
            ModbusServer modbusServer = new ModbusServer();
            modbusServer.Listen();
          
            Console.WriteLine($"Modbus Server is running and has IP Address: " +
                $"{ IpLocalizer.GetLocalIPAddress() }... UnitIdentifier is " +
                $"{ modbusServer.UnitIdentifier } ");


            Console.WriteLine("Do You wish to connect? ");
            char decision = Console.ReadKey().KeyChar;

            if (decision == 'y')
            {
                client.Connect();
                Console.WriteLine("\nHello!");
            }
            else
                Console.WriteLine("fail");

            //TODO: (5) Create logging functionality: Add infinite loop that handle the
            //event when someone connect to the server, read/write some data etc.; 


            Console.ReadKey();
        }
    }
}