using Blade.Grpc.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Grpc.LoadBalancer
{
    internal interface ILoadBalancerFactory
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<ILoadBalancer> Get(DownstreamProvider provider, ServiceProviderConfiguration config);
    }
}
