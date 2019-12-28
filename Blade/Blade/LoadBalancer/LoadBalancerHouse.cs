using Blade.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    internal class LoadBalancerHouse : ILoadBalancerHouse
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
            string key = CreateKey(downstream, config);
            if (_loadBalancers.TryGetValue(key, out var loadBalancer))
            { 
                if (downstream.LoadBalancerOptions.Type != loadBalancer.GetType().Name)
                {
                    loadBalancer = await _factory.Get(downstream, config);
                    AddLoadBalancer(key, loadBalancer);
                }
                balancer = loadBalancer;
                return balancer;
            }
            balancer = await _factory.Get(downstream, config);
            AddLoadBalancer(key, balancer);
            return balancer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="downstream"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task Remove(DownstreamProvider downstream, ServiceProviderConfiguration config)
        {
            string key = CreateKey(downstream, config);

            _loadBalancers.TryRemove(key,out var _);

            await Task.CompletedTask;
        }

        #region private

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loadBalancer"></param>
        private void AddLoadBalancer(string key, ILoadBalancer loadBalancer)
        {
            _loadBalancers.AddOrUpdate(key, loadBalancer, (x, y) => loadBalancer);
        }

        private string CreateKey(DownstreamProvider downstream, ServiceProviderConfiguration configuration)
        {
            return $"{downstream.ServiceName}|{configuration.Host}:{configuration.Port}";
        }

        #endregion
    }
}
