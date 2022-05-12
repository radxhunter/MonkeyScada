using CommunicationManager.Api.Modbus.Models;

namespace CommunicationManager.Api.Modbus.Services
{
    internal interface IModbusCommunicator
    {
        IEnumerable<string> GetSensorNames();
        IAsyncEnumerable<MeasurementPair<double, long>> StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        event EventHandler<MeasurementPair<double, long>>? MeasurementUpdated;
    }
}