using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration
{
    public class DownstreamProvider
    {
        public DownstreamProvider(
            string serviceName, 
            LoadBalancerOptions loadBalancerOptions)
        {
            ServiceName = serviceName;
            LoadBalancerOptions = loadBalancerOptions; 
        }
         

        public LoadBalancerOptions LoadBalancerOptions { get; }

        public string ServiceName { get; set; }
    }
}
