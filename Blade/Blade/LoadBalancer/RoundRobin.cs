using Blade.Grpc.Configuration;
using Blade.Grpc.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Grpc.LoadBalancer
{
    /// <summary>
    /// 轮询
    /// </summary>
    public class RoundRobin : ILoadBalancer
    {

        private readonly List<Service> _services;
        private static readonly object _syncLock = new object();
        private int _last;


        public RoundRobin(List<Service> services)
        {
            _services = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Task<ServiceHostAndPort> Lease(ServiceProviderConfiguration config)
        {
            lock (_syncLock)
            {
                if (_last >= _services.Count)
                {
                    _last = 0;
                }

                var next = _services[_last];
                _last++;
                return Task.FromResult(next.HostAndPort);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostAndPort"></param>
        public void Release(ServiceHostAndPort hostAndPort)
        {

        }
    }
}
