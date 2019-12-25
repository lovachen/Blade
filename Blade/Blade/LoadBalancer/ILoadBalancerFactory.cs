using Blade.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    public interface ILoadBalancerFactory
    {
        Task<ILoadBalancer> Get(DownstreamProvider provider, ServiceProviderConfiguration config);
    }
}
