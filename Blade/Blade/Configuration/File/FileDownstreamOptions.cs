using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.File
{
    public class FileDownstreamOptions
    {
        public FileLoadBalancerOptions LoadBalancerOptions { get; set; }

        public string ServiceName { get; set; }
    }
}
