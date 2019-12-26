﻿using Blade.Configuration.Create;
using Blade.Configuration.File;
using Blade.Grpc;
using Blade.Infrastructure;
using Blade.LoadBalancer;
using Blade.ServiceDiscovery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.DependencyInjection
{
    public class BladeBuilder : IBladeBuilder
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration {get;}

        public BladeBuilder(IServiceCollection services, IConfiguration configurationRoot)
        {
            Configuration = configurationRoot;
            Services = services; 
            Services.Configure<FileConfiguration>(configurationRoot);


            Services.TryAddSingleton<IBladeFactory, BladeFactory>();
            Services.TryAddSingleton<IServiceDiscoveryProviderFactory, ServiceDiscoveryProviderFactory>();
            Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.TryAddSingleton<ILoadBalancerHouse, LoadBalancerHouse>();
            Services.TryAddSingleton<IBladeGrpcFactory, BladeGrpcFactory>();
            Services.TryAddSingleton<IDownstreamKeyCreator, DownstreamKeyCreator>();
            Services.TryAddSingleton<IServiceProviderConfigurationCreator, ServiceProviderConfigurationCreator>();
            Services.AddMemoryCache();

        }


    }
}
