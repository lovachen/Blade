using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.File
{
    public class FileGlobalConfiguration
    {
        public FileGlobalConfiguration()
        {
            ServiceDiscoveryProvider = new FileServiceDiscoveryProvider();
            LoadBalancerOptions = new FileLoadBalancerOptions();
            Providers = new List<DownstreamProvider>();

        }

        public FileServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }

        public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DownstreamProvider> Providers { get; set; }

    }
}
