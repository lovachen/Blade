using Blade.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blade.Configuration.Create
{
    /// <summary>
    /// 
    /// </summary>
    public class DownstreamProviderCreate : IDownstreamProviderCreate
    {
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public List<DownstreamProvider> Create(InternalConfiguration config)
        {
            return config.GlobalConfiguration.Downstream
                 .Select(downstream =>
                 {
                     return Build(downstream, config.GlobalConfiguration);
                 }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public DownstreamProvider Create(InternalConfiguration config, string serviceName)
        {
            var downstream = config.GlobalConfiguration.Downstream.Where(o => o.ServiceName == serviceName).FirstOrDefault();
            if (downstream == null)
                throw new Exception($"{nameof(downstream)} 构建错误，ServiceName：{serviceName}未找到");


            return Build(downstream, config.GlobalConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private DownstreamProvider Build(FileDownstreamOptions downstream, FileGlobalConfiguration configuration)
        {
            LoadBalancerOptions loadBalancer;
            if (downstream.LoadBalancerOptions == null)
            { 
                loadBalancer = new LoadBalancerOptions(configuration.LoadBalancerOptions?.Type, configuration.LoadBalancerOptions?.Key);
            }
            else
            {
                loadBalancer = new LoadBalancerOptions(downstream.LoadBalancerOptions.Type, downstream.LoadBalancerOptions.Key);
            } 
            return new DownstreamProvider(downstream.ServiceName, loadBalancer);
        }

    }
}
