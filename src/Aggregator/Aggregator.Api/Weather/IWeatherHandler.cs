using Aggregator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.Api.Weather
{
    internal interface IWeatherHandler
    {
        Task HandleAsync(WeatherData weatherData);
    }
}
