using Aggregator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.Modbus
{
    internal interface IModbusHandler
    {
        Task HandleAsync(MeasurementPair<double, long> measurementPair);
    }
}
