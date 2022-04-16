using MonkeyScada.Shared.Serialization;
using MonkeyScada.Shared.Streaming;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.Redis.Streaming
{
    internal sealed class RedisStreamSubscriber : IStreamSubscriber
    {
        private readonly ISubscriber _subscriber;
        private readonly ISerializer _serializer;

        public RedisStreamSubscriber(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
        {
            _subscriber = connectionMultiplexer.GetSubscriber();
            _serializer = serializer;
        }

        public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class
        {
            return _subscriber.SubscribeAsync(topic, (_, data) =>
            {
                var payload = _serializer.Deserialize<T>(data);
                if (payload is null)
                {
                    return;
                }

                handler(payload);
            });
        }
    }
}
