using Blade.Grpc.Configuration;
using Blade.Grpc.Configuration.File;
using Blade.Grpc.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Grpc.LoadBalancer
{
    /// <summary>
    /// 负载均衡
    /// </summary>
    public interface ILoadBalancer
    {
        /// <summary>
        /// 租界主机
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<ServiceHostAndPort> Lease(ServiceProviderConfiguration config);

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="hostAndPort"></param>
        void Release(ServiceHostAndPort hostAndPort);
    }
}
