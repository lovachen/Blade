using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceProviderConfiguration
    {
        public ServiceProviderConfiguration(string type, string host, int port, string token, int pollingInterval)
        { 
            Host = host;
            Port = port; 
            Token = token;
            Type = type;
            PollingInterval = pollingInterval; 
        }

        public string Host { get; }

        public int Port { get; }

        public string Type { get; }

        public string Token { get; }
         

        public int PollingInterval { get; }
         
    }
}
