using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.File
{
    public class FileGlobalConfiguration
    {
        public FileGlobalConfiguration()
        {
            ServiceDiscoveryProvider = new List<FileServiceDiscoveryProvider>();
            LoadBalancerOptions = new FileLoadBalancerOptions();
            Providers = new List<DownstreamProvider>();

        }

        public List<FileServiceDiscoveryProvider> ServiceDiscoveryProvider { get; set; }

        public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DownstreamProvider> Providers { get; set; }

    }
}
