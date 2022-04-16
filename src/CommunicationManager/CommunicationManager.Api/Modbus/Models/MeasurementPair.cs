using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Modbus.Models
{
    public sealed record MeasurementPair(string SensorName, double Value, long Timestamp);
}
