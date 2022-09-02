using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Api.Models;

namespace Weather.Api.Services
{
internal interface IWeatherFeed
{
    IAsyncEnumerable<WeatherData> SubscribeAsync(string location, CancellationToken cancellationToken);
}
}
