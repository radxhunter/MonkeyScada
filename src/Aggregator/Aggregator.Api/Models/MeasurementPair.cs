using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.Models
{
    internal sealed record MeasurementPair(string SensorName, double Value, long Timestamp);
}
