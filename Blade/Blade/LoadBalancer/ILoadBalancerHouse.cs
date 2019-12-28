using Grpc.Blade.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grpc.Blade.LoadBalancer
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="downstream"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task Remove(DownstreamProvider downstream, ServiceProviderConfiguration config);

    }
}
