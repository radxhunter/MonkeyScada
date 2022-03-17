using CommunicationManager.Api.Models;

namespace CommunicationManager.Api.Services
{
    public interface IDeviceEnrollmentService
    {
        IList<WaterConsumer> WaterConsumers { get; }

        void AddMeasurement(WaterConsumer waterConsumer);
    }
}