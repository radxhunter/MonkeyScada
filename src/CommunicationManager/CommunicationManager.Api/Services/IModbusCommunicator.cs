
namespace CommunicationManager.Api.Services
{
    internal interface IModbusCommunicator
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}