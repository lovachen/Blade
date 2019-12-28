namespace Blade.XUnitTest
{
    using Blade.ServiceDiscovery.Providers;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Blade.Provider.Consul;
    using Xunit.Abstractions;
    using Blade.Grpc;
    using Blade.LoadBalancer;
    using Blade.Configuration.File;
    using Blade.Configuration.Create;
    using Blade.ServiceDiscovery;

    public class TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider;
        public ITestOutputHelper OutputHelper;

        public TestBase()
        {
            IServiceCollection services = new ServiceCollection();
             
            services.AddSingleton<IConsulClientFactory, ConsulClientFactory>();
            services.AddSingleton<IServiceDiscoveryProvider, Consul>();

            var config = new ConsulRegistryConfiguration("192.168.0.109", 8500, "grpc.user", null);

            services.Configure<FileConfiguration>(o=> {
                o.GlobalConfiguration = new FileGlobalConfiguration() { 
                     ServiceDiscoveryProvider = new FileServiceDiscoveryProvider()
                     {
                          Host= "192.168.0.109",
                          Port = 8500,
                          PollingInterval=1000,
                     }
                };
            });

            services.AddSingleton<ConsulRegistryConfiguration>(config);
            services.AddSingleton<IBladeGrpcFactory, BladeGrpcFactory>();
            //services.AddSingleton<ILoadBalancerHouse, LoadBalancerHouse>();
            //services.AddSingleton<ILoadBalancerFactory, LoadBalancerFactory>();
            services.AddSingleton<IDownstreamProviderCreate, DownstreamProviderCreate>(); 
            services.AddSingleton<IServiceProviderConfigurationCreator, ServiceProviderConfigurationCreator>();
            ServiceProvider = services.BuildServiceProvider();
        }

    }
}
