namespace CommunicationManager.Api.SerialPortConnector.Services
{
    public interface ISerialPortConnectorService
    {
        void Send(string command);
        void Read();
    }
}