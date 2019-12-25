using Blade.Configuration;
using Blade.ServiceDiscovery.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.ServiceDiscovery
{
    /// <summary>
    /// 定义委托 获取 IServiceDiscoveryProvider
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public delegate IServiceDiscoveryProvider ServiceDiscoveryFinderDelegate(IServiceProvider provider, ServiceProviderConfiguration config, DownstreamProvider downstream);
}
