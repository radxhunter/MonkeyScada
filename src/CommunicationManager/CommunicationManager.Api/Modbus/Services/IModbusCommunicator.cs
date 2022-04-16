using CommunicationManager.Api.Modbus.Models;

namespace CommunicationManager.Api.Modbus.Services
{
    internal interface IModbusCommunicator
    {
        IEnumerable<string> GetSensorNames();
        IAsyncEnumerable<MeasurementPair> StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        event EventHandler<MeasurementPair>? MeasurementUpdated;
    }
}