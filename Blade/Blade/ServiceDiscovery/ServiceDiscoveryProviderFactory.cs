using Blade.Configuration;
using Blade.ServiceDiscovery.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Blade.ServiceDiscovery
{
    public class ServiceDiscoveryProviderFactory : IServiceDiscoveryProviderFactory
    {
        private readonly ServiceDiscoveryFinderDelegate _delegates;
        private readonly IServiceProvider _provider;

        public ServiceDiscoveryProviderFactory(IServiceProvider provider)
        {
            _provider = provider;
            _delegates = provider.GetService<ServiceDiscoveryFinderDelegate>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceConfig"></param>
        /// <param name="downstream"></param>
        /// <returns></returns>
        public IServiceDiscoveryProvider Get(ServiceProviderConfiguration serviceConfig, DownstreamProvider downstream)
        {
            return GetServiceDiscoveryProvider(serviceConfig, downstream);
        }



        private IServiceDiscoveryProvider GetServiceDiscoveryProvider(ServiceProviderConfiguration config, DownstreamProvider downstream)
        {
            if (_delegates != null)
            {
                var provider = _delegates.Invoke(_provider, config, downstream);
                if (provider.GetType().Name.ToLower() == config.Type.ToLower())
                {
                    return provider;
                }
            }
            throw new Exception("ServiceDiscoveryProvider ：Type 配置错误");
        }


    }
}
