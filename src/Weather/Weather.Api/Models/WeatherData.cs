using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Api.Models
{
    internal record WeatherData(string Location, double Temperature, double Humidity, double Wind, string Condition)
    {

    }
}
