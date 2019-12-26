namespace Blade.Provider.Consul
{
    using Blade.Provider.Consul;
    using Blade.ServiceDiscovery;
    using Blade.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

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
