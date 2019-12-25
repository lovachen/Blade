using Blade.Configuration.File;
using Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    /// <summary>
    /// 负载均衡
    /// </summary>
    public interface ILoadBalancer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<ServiceHostAndPort> Lease(FileServiceDiscoveryProvider provider);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostAndPort"></param>
        void Release(ServiceHostAndPort hostAndPort);
    }
}
