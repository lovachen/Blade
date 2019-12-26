using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration
{
    public class DownstreamProvider
    {
        public DownstreamProvider(
            string serviceName, 
            string loadBalancerKey,
            LoadBalancerOptions loadBalancerOptions)
        {
            ServiceName = serviceName;
            LoadBalancerKey = loadBalancerKey;
            LoadBalancerOptions = loadBalancerOptions; 
        }
         

        public LoadBalancerOptions LoadBalancerOptions { get; }

        public string LoadBalancerKey { get; set; }

        public string ServiceName { get; set; }
    }
}
