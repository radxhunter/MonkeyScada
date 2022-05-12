using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Weather.Api.Models;

namespace Weather.Api.Services;

internal sealed class WeatherFeed : IWeatherFeed
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherFeed> _logger;

    public WeatherFeed(HttpClient client,
        IConfiguration configuration,
        ILogger<WeatherFeed> logger)
    {
        _client = client;
        _configuration = configuration;
        _logger = logger;
    }

    public async IAsyncEnumerable<WeatherData> SubscribeAsync(string location, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string apiKey = _configuration.GetSection("WeatherApi").GetSection("ApiKey").Value;
        string apiUrl = _configuration.GetSection("WeatherApi").GetSection("ApiUrl").Value;
        int intervalOfPollingInSeconds = _configuration.GetSection("WeatherApi").GetValue<int>("IntervalOfPollingInSeconds");

        var url = $"{apiUrl}?key={apiKey}&q={location}&aqi=no";
        while (!cancellationToken.IsCancellationRequested)
        {
            WeatherApiResponse? response;

            try
            {
                response = await _client.GetFromJsonAsync<WeatherApiResponse>(url, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }

            if (response is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(intervalOfPollingInSeconds), cancellationToken);
                continue;
            }

            yield return new WeatherData($"{response.Location.Name}, {response.Location.Country}",
                response.Current.TempC, response.Current.Humidity, response.Current.WindKph,
                response.Current.Condition.Text);

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    private record WeatherApiResponse(Location Location, Weather Current);

    private record Location(string Name, string Country);

    private record Condition(string Text);

    private record Weather([property: JsonPropertyName("temp_c")] double TempC, double Humidity, Condition Condition,
        [property: JsonPropertyName("wind_kph")] double WindKph);
}

