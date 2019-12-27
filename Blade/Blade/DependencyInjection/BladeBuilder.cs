using Blade.Configuration.Create;
using Blade.Configuration.File;
using Blade.Grpc;
using Blade.Infrastructure;
using Blade.LoadBalancer;
namespace Blade.DependencyInjection
{
    using Blade.ServiceDiscovery;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using System;

    public class BladeBuilder : IBladeBuilder
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration {get;}

        public BladeBuilder(IServiceCollection services, IConfiguration configurationRoot)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            Configuration = configurationRoot;
            Services = services; 
            Services.Configure<FileConfiguration>(configurationRoot);


            Services.TryAddSingleton<IBladeFactory, BladeFactory>(); 
            Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.TryAddSingleton<ILoadBalancerHouse, LoadBalancerHouse>();
            Services.TryAddSingleton<IBladeGrpcFactory, BladeGrpcFactory>();
            Services.TryAddSingleton<IServiceProviderConfigurationCreator, ServiceProviderConfigurationCreator>();
            Services.TryAddSingleton<ILoadBalancerFactory, LoadBalancerFactory>();
            services.TryAddSingleton<IDownstreamProviderCreate, DownstreamProviderCreate>();
            services.TryAddSingleton<IInternalConfigurationCreate, InternalConfigurationCreate>();

            Services.AddMemoryCache();
            Services.AddLogging();
        }

        

    }
}
