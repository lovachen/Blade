using Blade.Grpc.LoadBalancer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Values
{
    /// <summary>
    /// 
    /// </summary>
    internal class LoadBalancerHttpItems
    {
        public LoadBalancerHttpItems(ServiceHostAndPort hostAndPort,
            ILoadBalancer loadBalancer)
        {
            ServiceHostAndPort = hostAndPort;
            LoadBalancer = loadBalancer;
        }

        public ServiceHostAndPort ServiceHostAndPort { get; set; }

        public ILoadBalancer LoadBalancer { get; set; }
    }
}
