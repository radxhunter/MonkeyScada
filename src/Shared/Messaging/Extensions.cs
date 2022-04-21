using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.Messaging
{
    public static class Extensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            return services
                .AddSingleton<IMessagePublisher, DefaultMessagePublisher>()
                .AddSingleton<IMessageSubscriber, DefaultMessageSubscriber>();
        }
    }
}
