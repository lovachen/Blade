using Blade.DependencyInjection;
using Blade.ServiceDiscovery;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Provider.Consul
{
    public static class BladeBuilderExtensions
    {
        /// <summary>
        /// 注册Consul
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IBladeBuilder AddConsul(this IBladeBuilder builder)
        {
            builder.Services.AddSingleton<ServiceDiscoveryFinderDelegate>(ConsulProviderFactory.Get);
            builder.Services.AddSingleton<IConsulClientFactory, ConsulClientFactory>();

            return builder;
        }
    }
}
