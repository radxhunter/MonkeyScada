using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public int ConsumerId { get; set; }
        public double ActualWaterConsumingPerDay { get; set; }
    }
}
