using FluentModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ModbusStandardLibrary
{
    public class ModbusStandardService : IModbusStandardService
    {
        private ModbusTcpClient client = new ModbusTcpClient();
        private const int UnitIdentifier = 0x01;

        public ModbusStandardService()
        {
            client.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.143"), 502), ModbusEndianness.BigEndian);
        }
        public void SendModbusRequest(short[] dataSet, int startingAddress)
        {
            client.WriteMultipleRegisters(UnitIdentifier, startingAddress, dataSet);
        }

        public IEnumerable<short> GetModbusValues(int startingAddress, int count)
        {
            var shortDataResult = client.ReadHoldingRegisters<short>(UnitIdentifier, startingAddress, count);

            return shortDataResult.ToArray();
        }
    }
}
