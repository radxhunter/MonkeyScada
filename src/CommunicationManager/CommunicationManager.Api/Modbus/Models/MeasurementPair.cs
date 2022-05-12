using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Modbus.Models
{
    public sealed record MeasurementPair<TValue, TTime>(string SensorName, TValue Value, TTime Time);
}
