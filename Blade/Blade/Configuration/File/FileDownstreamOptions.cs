using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.File
{
    public class FileDownstreamOptions
    {
        public LoadBalancerOptions LoadBalancerOptions { get; }

        public string ServiceName { get; set; }
    }
}
