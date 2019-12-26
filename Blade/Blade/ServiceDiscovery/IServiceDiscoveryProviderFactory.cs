using Blade.Configuration;
using Blade.ServiceDiscovery.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.ServiceDiscovery
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceDiscoveryProviderFactory
    {
        /// <summary>
        /// 调用委托获取IServiceDiscoveryProvider实例
        /// </summary>
        /// <param name="serviceConfig"></param>
        /// <param name="downstream"></param>
        /// <returns></returns>
        IServiceDiscoveryProvider Get(ServiceProviderConfiguration serviceConfig, DownstreamProvider downstream);

    }
}
