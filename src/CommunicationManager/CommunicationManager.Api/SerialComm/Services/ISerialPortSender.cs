namespace CommunicationManager.Api.SerialComm.Services
{
    public interface ISerialPortSender
    {
        void Send(string command);
    }
}