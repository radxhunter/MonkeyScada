using Aggregator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.SerialComm
{
    internal interface ISerialHandler
    {
        Task HandleAsync(MeasurementPair<string, DateTime> measurementPair);
    }
}
