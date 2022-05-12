namespace CommunicationManager.Api.SerialComm.Services
{
    public interface ISerialPortConnectorService
    {
        void Send(string command);
    }
}