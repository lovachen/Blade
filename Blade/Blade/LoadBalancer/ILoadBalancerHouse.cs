using Blade.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    public interface ILoadBalancerHouse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<ILoadBalancer> Get(DownstreamProvider provider, ServiceProviderConfiguration config);
    }
}
