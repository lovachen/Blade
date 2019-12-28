using Blade.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Blade.Configuration.File;
using Blade.Configuration.Create;
using Blade.ServiceDiscovery.Providers;
using Blade.Configuration;
using Blade.LoadBalancer;
using Blade.Grpc;
using Blade.Values;

namespace Microsoft.AspNetCore.Builder
{
    public static class BladeMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseBlade(this IApplicationBuilder app)
        {
            CreateConfiguration(app);
            app.UseMiddleware<BladeMiddleware>();
            return app;
        }

        #region private

        /// <summary>
        /// 监听json配置文件更改
        /// </summary>
        /// <param name="builder"></param>
        private static void CreateConfiguration(IApplicationBuilder builder)
        {
            var configurationCreate = builder.ApplicationServices.GetService<IInternalConfigurationCreate>();
            var serviceDiscoveryProvider = builder.ApplicationServices.GetService<IServiceDiscoveryProvider>();
            var fileConfig = builder.ApplicationServices.GetService<IOptionsMonitor<FileConfiguration>>();

            configurationCreate.AddOrReplace(fileConfig.CurrentValue);

            fileConfig.OnChange(config =>
            {
                configurationCreate.AddOrReplace(config);
                serviceDiscoveryProvider.ClearListner();
                StartListner(builder, serviceDiscoveryProvider, config);
            });

            //
            if (fileConfig.CurrentValue.GlobalConfiguration.ServiceDiscoveryProvider.Listening)
            {
                StartListner(builder, serviceDiscoveryProvider, fileConfig.CurrentValue);
            }
        }

        private static void StartListner(IApplicationBuilder builder, IServiceDiscoveryProvider discoveryProvider, FileConfiguration fileConfiguration)
        {
            var loadBalancerHouse = builder.ApplicationServices.GetService<ILoadBalancerHouse>();
            var grpcFactory = builder.ApplicationServices.GetService<IBladeGrpcFactory>();

            var global = fileConfiguration.GlobalConfiguration;
            string host = global.ServiceDiscoveryProvider.Host;
            int port = global.ServiceDiscoveryProvider.Port;
            string token = global.ServiceDiscoveryProvider.Token;
            int pollingInterval = global.ServiceDiscoveryProvider.PollingInterval;
            if (pollingInterval == 0) pollingInterval = 100;
            foreach (var item in fileConfiguration.GlobalConfiguration.Downstream)
            {
                var config = new ServiceDiscoveryConfiguration(host, port, token, pollingInterval, item.ServiceName);
                discoveryProvider.AddListener(config);
                config.Callback.Add(() =>
                {
                    loadBalancerHouse.Remove(new DownstreamProvider(config.ServiceName, null), new ServiceProviderConfiguration(config.Host, config.Port, config.Token));
                });
                config.Callback.Add(() =>
                {
                    grpcFactory.Remove(new ServiceHostAndPort(config.Host,config.Port));
                });
            }
        }

        #endregion
    }
}
