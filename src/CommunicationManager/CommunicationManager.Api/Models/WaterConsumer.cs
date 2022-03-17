using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager.Api.Models
{
    public class WaterConsumer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double DesiredWaterConsumingPerDay { get; set; }
    }
}
