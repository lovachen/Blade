using Blade.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blade.Configuration.Create
{
    public class DownstreamProviderCreate : IDownstreamProviderCreate
    {
        private IDownstreamKeyCreator _downstreamKeyCreator;

        public DownstreamProviderCreate(IDownstreamKeyCreator keyCreator)
        {
            _downstreamKeyCreator = keyCreator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public List<DownstreamProvider> Create(FileConfiguration config)
        {
            return config.GlobalConfiguration.Providers
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
        public DownstreamProvider Create(FileConfiguration config, string serviceName)
        {
            var downstream = config.GlobalConfiguration.Providers.Where(o => o.ServiceName == serviceName).FirstOrDefault();
            if (downstream == null)
                throw new Exception($"{nameof(downstream)} 构建错误，ServiceName：{serviceName}未找到");
            return Build(downstream, config.GlobalConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private DownstreamProvider Build(DownstreamProvider downstream, FileGlobalConfiguration configuration)
        {
            LoadBalancerOptions loadBalancer = downstream.LoadBalancerOptions;
            if (loadBalancer == null && configuration.LoadBalancerOptions != null)
            {
                loadBalancer = new LoadBalancerOptions(configuration.LoadBalancerOptions.Type, configuration.LoadBalancerOptions.Key);
            }
            string loadBalanceKey = _downstreamKeyCreator.Create(downstream);
            return new DownstreamProvider(downstream.ServiceName, loadBalanceKey, loadBalancer);
        }

    }
}
