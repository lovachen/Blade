using Blade.Configuration;
using Blade.ServiceDiscovery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    /// <summary>
    /// 
    /// </summary>
    public class LoadBalancerFactory : ILoadBalancerFactory
    {
        private readonly IServiceDiscoveryProviderFactory _serviceProviderFactory;

        public LoadBalancerFactory(IServiceDiscoveryProviderFactory serviceDiscoveryProviderFactory)
        {
            _serviceProviderFactory = serviceDiscoveryProviderFactory;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="downstream"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<ILoadBalancer> Get(DownstreamProvider downstream, ServiceProviderConfiguration config)
        { 
            var serviceProvider = _serviceProviderFactory.Get(config, downstream);

            var services = await serviceProvider.Get();

            switch (downstream.LoadBalancerOptions?.Type)
            {
                case nameof(LeastConnection):
                    return new LeastConnection(services);
                default:
                    return new NoLoadBalancer(services);
            } 
        }
    }
}
