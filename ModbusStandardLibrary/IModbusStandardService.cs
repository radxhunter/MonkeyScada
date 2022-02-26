using System.Collections.Generic;

namespace ModbusStandardLibrary
{
    public interface IModbusStandardService
    {
        IEnumerable<short> GetModbusValues(int startingAddress, int count);
        void SendModbusRequest(short[] dataSet, int startingAddress);
    }
}