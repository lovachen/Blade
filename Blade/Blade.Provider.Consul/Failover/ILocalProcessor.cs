using Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Provider.Consul.Failover
{
    public interface ILocalProcessor
    {
        /// <summary>
        /// 获取配置文件内容
        /// </summary>
        /// <returns></returns>
        Task<List<Service>> GetConfigAsync(string host, int port, string serviceName);

        /// <summary>
        /// 存储配置文件内容
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="group"></param>
        /// <param name="tenant"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task SaveConfigAsync(string host, int port, string serviceName, List<Service> services);

        /// <summary>
        /// 移除配置本地存储值
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="group"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        Task RemoveConfigAsync(string host, int port, string serviceName);
    }
}
