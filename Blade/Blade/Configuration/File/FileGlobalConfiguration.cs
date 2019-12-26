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
            Downstream = new List<FileDownstreamOptions>();

        }

        /// <summary>
        /// 
        /// </summary>

        public FileServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<FileDownstreamOptions> Downstream { get; set; }

    }
}
