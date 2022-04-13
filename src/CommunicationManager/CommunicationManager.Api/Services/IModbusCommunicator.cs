
using CommunicationManager.Api.Models;

namespace CommunicationManager.Api.Services
{
    internal interface IModbusCommunicator
    {
        IAsyncEnumerable<MeasurementPair> StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}