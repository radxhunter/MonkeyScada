using CommunicationManager.Api.Helpers;
using CommunicationManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Services
{
    public class MeasurementService : IMeasurementService
    {
        public IList<Measurement> Measurements { get; private set; }

        public MeasurementService()
        {
            Measurements = new List<Measurement>();
        }

        public void AddMeasurement(Measurement measurement)
        {
            Measurements.Add(measurement);
        }
    }
}
