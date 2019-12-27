namespace Blade.Provider.Consul
{
    using Blade.Provider.Consul;
    using Blade.ServiceDiscovery;
    using Blade.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Blade.Provider.Consul.Failover;
    using Blade.ServiceDiscovery.Providers;

    public static class BladeBuilderExtensions
    {
        /// <summary>
        /// 注册Consul
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IBladeBuilder AddConsul(this IBladeBuilder builder)
        { 
            builder.Services.AddSingleton<IConsulClientFactory, ConsulClientFactory>();
            builder.Services.AddSingleton<ILocalProcessor, LocalProcessor>();
            builder.Services.AddSingleton<IServiceDiscoveryProvider, Consul>();

            return builder;
        }
    }
}
