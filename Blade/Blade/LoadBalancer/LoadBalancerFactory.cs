using Blade.Grpc.Configuration;
using Blade.Grpc.ServiceDiscovery;
using Blade.Grpc.ServiceDiscovery.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Grpc.LoadBalancer
{
    /// <summary>
    /// 
    /// </summary>
    internal class LoadBalancerFactory : ILoadBalancerFactory
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

            return (downstream.LoadBalancerOptions?.Type) switch
            {
                nameof(LeastConnection) => new LeastConnection(services),
                nameof(RoundRobin)=>new RoundRobin(services),
                _ => new NoLoadBalancer(services),
            };
        }
    }
}
