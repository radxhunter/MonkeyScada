using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.HTTP
{
    public static class Extensions
    {
        public static IServiceCollection AddHttpApiClient<TInterface, TClient>(this IServiceCollection services)
            where TInterface : class where TClient : class, TInterface
        {
            services.AddHttpClient<TInterface, TClient>();

            return services;
        }
    }
}
