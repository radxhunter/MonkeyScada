using CommunicationManager.Api.Models;

namespace CommunicationManager.Api.Services
{
    public interface IMeasurementService
    {
        IList<Measurement> Measurements { get; }

        void AddMeasurement(Measurement measurement);
    }
}