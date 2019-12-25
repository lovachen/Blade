using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.File
{
    public class FileLoadBalancerOptions
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public int Expiry { get; set; }
    }
}
