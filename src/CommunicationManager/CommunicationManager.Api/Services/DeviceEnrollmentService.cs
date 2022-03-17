using CommunicationManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Services
{
    public class DeviceEnrollmentService : IDeviceEnrollmentService
    {
        public IList<WaterConsumer> WaterConsumers { get; private set; }

        public DeviceEnrollmentService()
        {
            WaterConsumers = new List<WaterConsumer>();
        }

        public void AddMeasurement(WaterConsumer waterConsumer)
        {
            WaterConsumers.Add(waterConsumer);
        }
    }
}
