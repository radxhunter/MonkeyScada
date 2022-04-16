using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.Serialization
{
    public static class Extensions
    {
        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            return services.AddSingleton<ISerializer, SystemTextJsonSerializer>();
        }
    }
}
