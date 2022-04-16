using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.Streaming
{
    public static class Extensions
    {
        public static IServiceCollection AddStreaming(this IServiceCollection services)
        {
            return services
                .AddSingleton<IStreamPublisher, DefaultStreamPublisher>()
                .AddSingleton<IStreamSubscriber, DefaultStreamSubscriber>();
        }
    }
}
