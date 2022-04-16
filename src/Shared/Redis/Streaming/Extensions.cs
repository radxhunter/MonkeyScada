using Microsoft.Extensions.DependencyInjection;
using MonkeyScada.Shared.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.Redis.Streaming
{
    public static class Extensions
    {
        public static IServiceCollection AddRedisStreaming(this IServiceCollection services)
        {
            return services
             .AddSingleton<IStreamPublisher, RedisStreamPublisher>()
                .AddSingleton<IStreamSubscriber, RedisStreamSubscriber>();
        }
    }
}
