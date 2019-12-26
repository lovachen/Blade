using Blade.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    public class LoadBalancerHouse : ILoadBalancerHouse
    {
        private readonly ILoadBalancerFactory _factory;
        private readonly ConcurrentDictionary<string, ILoadBalancer> _loadBalancers;

        public LoadBalancerHouse(ILoadBalancerFactory factory)
        {
            _factory = factory;
            _loadBalancers = new ConcurrentDictionary<string, ILoadBalancer>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<ILoadBalancer> Get(DownstreamProvider downstream, ServiceProviderConfiguration config)
        {
            ILoadBalancer balancer;

            if (_loadBalancers.TryGetValue(downstream.LoadBalancerKey, out var loadBalancer))
            {
                loadBalancer = _loadBalancers[downstream.LoadBalancerKey];
                if (downstream.LoadBalancerOptions.Type != loadBalancer.GetType().Name)
                {
                    loadBalancer = await _factory.Get(downstream, config);
                    balancer = loadBalancer;
                    AddLoadBalancer(downstream.LoadBalancerKey, loadBalancer);
                }
                balancer = loadBalancer;
                return balancer;
            }
            balancer = await _factory.Get(downstream, config);
            AddLoadBalancer(downstream.LoadBalancerKey, balancer);
            return balancer;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loadBalancer"></param>
        private void AddLoadBalancer(string key, ILoadBalancer loadBalancer)
        {
            _loadBalancers.AddOrUpdate(key, loadBalancer, (x, y) => loadBalancer);
        }
    }
}
