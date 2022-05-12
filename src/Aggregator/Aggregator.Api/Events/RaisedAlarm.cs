using MonkeyScada.Shared.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.Events
{
    internal record RaisedAlarm(string AlarmId, string SensorName) : IMessage;
}
