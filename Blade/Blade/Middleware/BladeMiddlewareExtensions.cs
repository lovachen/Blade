﻿using Blade.Grpc;
using Blade.Grpc.Configuration;
using Blade.Grpc.Configuration.Create;
using Blade.Grpc.Configuration.File;
using Blade.Grpc.LoadBalancer;
using Blade.Grpc.Middleware;
using Blade.Grpc.ServiceDiscovery.Providers;
using Blade.Grpc.Values;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            if (fileConfig.CurrentValue.BladeGrpc.ServiceDiscoveryProvider.Listening)
            {
                StartListner(builder, serviceDiscoveryProvider, fileConfig.CurrentValue);
            }
        }

        private static void StartListner(IApplicationBuilder builder, IServiceDiscoveryProvider discoveryProvider, FileConfiguration fileConfiguration)
        {
            var loadBalancerHouse = builder.ApplicationServices.GetService<ILoadBalancerHouse>();
            var grpcFactory = builder.ApplicationServices.GetService<IBladeGrpcFactory>();

            var global = fileConfiguration.BladeGrpc;
            string host = global.ServiceDiscoveryProvider.Host;
            int port = global.ServiceDiscoveryProvider.Port;
            string token = global.ServiceDiscoveryProvider.Token;
            int pollingInterval = global.ServiceDiscoveryProvider.PollingInterval;
            if (pollingInterval == 0) pollingInterval = 1000;
            foreach (var item in fileConfiguration.BladeGrpc.Downstream)
            {
                var config = new ServiceDiscoveryConfiguration(host, port, token, pollingInterval, item.ServiceName);
                config.Callback.Add(() =>
                {
                    loadBalancerHouse.Remove(new DownstreamProvider(config.ServiceName, null), new ServiceProviderConfiguration(config.Host, config.Port, config.Token));
                });
                discoveryProvider.AddListener(config);
            }
        }

        #endregion
    }
}
