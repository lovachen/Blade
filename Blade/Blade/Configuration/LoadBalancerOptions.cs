using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration
{
    public class LoadBalancerOptions
    { 
        public LoadBalancerOptions(string type, string key)
        {
            Type = type;
            Key = key; 
        }

        public string Type { get; }

        public string Key { get; }

    }
}
