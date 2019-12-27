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
        public ServiceProviderConfiguration(string host, int port, string token)
        { 
            Host = host;
            Port = port; 
            Token = token;  
        }

        public string Host { get; }

        public int Port { get; }

        public string Token { get; }
         
    }
}
