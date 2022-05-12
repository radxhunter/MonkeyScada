using CommunicationManager.Api.Modbus.Models;

namespace CommunicationManager.Api.SerialComm.Services
{
    public interface ISerialPortCommunicator
    {
        IAsyncEnumerable<MeasurementPair<string, DateTime>> StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        event EventHandler<MeasurementPair<string, DateTime>>? MeasurementUpdated;
    }
}