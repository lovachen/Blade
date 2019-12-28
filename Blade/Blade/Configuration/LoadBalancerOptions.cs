using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration
{
    public class LoadBalancerOptions
    { 
        public LoadBalancerOptions(string type, string key)
        {
            Type = type;
            Key = key; 
        }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; }

    }
}
