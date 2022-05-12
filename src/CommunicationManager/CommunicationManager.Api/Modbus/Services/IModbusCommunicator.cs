using CommunicationManager.Api.Modbus.Models;

namespace CommunicationManager.Api.Modbus.Services
{
    internal interface IModbusCommunicator
    {
        IEnumerable<string> GetSensorNames();
        IAsyncEnumerable<MeasurementPair<double>> StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        event EventHandler<MeasurementPair<double>>? MeasurementUpdated;
    }
}