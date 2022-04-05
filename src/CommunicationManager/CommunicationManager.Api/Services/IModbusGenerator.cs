
namespace CommunicationManager.Api.Services
{
    internal interface IModbusGenerator
    {
        Task StartAsync();
        Task StopAsync();
    }
}