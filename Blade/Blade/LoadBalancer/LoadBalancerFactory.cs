using Blade.Configuration;
using Blade.ServiceDiscovery;
using Blade.ServiceDiscovery.Providers;
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
        private readonly IServiceDiscoveryProvider _serviceDiscoveryProvider;

        public LoadBalancerFactory(IServiceDiscoveryProvider serviceDiscoveryProvider)
        {
            _serviceDiscoveryProvider = serviceDiscoveryProvider;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="downstream"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<ILoadBalancer> Get(DownstreamProvider downstream, ServiceProviderConfiguration config)
        {
            var cfg = new ServiceDiscoveryConfiguration(config.Host, config.Port, config.Token, downstream.ServiceName);
            var services = await _serviceDiscoveryProvider.GetServices(cfg);

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
