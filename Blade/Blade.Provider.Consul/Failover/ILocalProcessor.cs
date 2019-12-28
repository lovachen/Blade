using Grpc.Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grpc.Blade.Provider.Consul.Failover
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILocalProcessor
    {
        /// <summary>
        /// 获取配置文件内容
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task<List<Service>> GetConfigAsync(string host, int port, string serviceName);

        /// <summary>
        /// 存储配置文件内容
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        Task SaveConfigAsync(string host, int port, string serviceName, List<Service> services);

        /// <summary>
        /// 移除配置本地存储值
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task RemoveConfigAsync(string host, int port, string serviceName);
    }
}
