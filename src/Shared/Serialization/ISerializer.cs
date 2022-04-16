using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonkeyScada.Shared.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T value) where T : class;
        T? Deserialize<T>(string value) where T : class;
    }

    internal sealed class SystemTextJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)}
        };

        public T? Deserialize<T>(string value) where T : class => JsonSerializer.Deserialize<T>(value, Options);

        public string Serialize<T>(T value) where T : class => JsonSerializer.Serialize(value, Options);
    }
}
