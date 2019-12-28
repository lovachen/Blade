namespace Grpc.Blade.Provider.Consul
{
    using Grpc.Blade.Provider.Consul;
    using Grpc.Blade.ServiceDiscovery;
    using Grpc.Blade.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Grpc.Blade.Provider.Consul.Failover;
    using Grpc.Blade.ServiceDiscovery.Providers;

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
