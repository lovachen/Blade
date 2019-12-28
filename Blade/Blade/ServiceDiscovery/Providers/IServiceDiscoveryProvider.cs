using Blade.Configuration;
using Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.ServiceDiscovery.Providers
{
    /// <summary>
    /// 服务发现提供者
    /// </summary>
    public interface IServiceDiscoveryProvider
    {
        /// <summary>
        /// 获取服务地址列表
        /// </summary>
        /// <returns></returns>
        Task<List<Service>> GetServices(ServiceDiscoveryConfiguration config);

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        void AddListener(ServiceDiscoveryConfiguration config);

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        void RemoveListner(ServiceDiscoveryConfiguration config);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        void ClearListner();
    }
}
