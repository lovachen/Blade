using Blade.Configuration;
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
        public Task<ILoadBalancer> Get(DownstreamProvider provider,ServiceProviderConfiguration config)
        {





        }
    }
}
