namespace Blade.Grpc.Provider.Consul
{
    using Blade.Grpc.Provider.Consul;
    using Blade.Grpc.ServiceDiscovery;
    using Blade.Grpc.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Blade.Grpc.Provider.Consul.Failover;
    using Blade.Grpc.ServiceDiscovery.Providers;

    /// <summary>
    /// 
    /// </summary>
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
