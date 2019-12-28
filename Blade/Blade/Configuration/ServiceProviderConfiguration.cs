using System;
using System.Collections.Generic;
using System.Text;

namespace Grpc.Blade.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceProviderConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="token"></param>
        public ServiceProviderConfiguration(string host, int port, string token)
        { 
            Host = host;
            Port = port; 
            Token = token;  
        }

        /// <summary>
        /// 
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; }
         
    }
}
