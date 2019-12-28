using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Provider.Consul
{
    /// <summary>
    /// 获取 Consul Client 工厂
    /// </summary>
    public interface IConsulClientFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        IConsulClient Get(ConsulRegistryConfiguration config);
    }
}
