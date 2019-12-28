using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration.File
{
    public class FileDownstreamOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }
    }
}
