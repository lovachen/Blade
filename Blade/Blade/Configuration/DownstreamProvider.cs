using System;
using System.Collections.Generic;
using System.Text;

namespace Grpc.Blade.Configuration
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
         
        /// <summary>
        /// 
        /// </summary>
        public LoadBalancerOptions LoadBalancerOptions { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }
    }
}
