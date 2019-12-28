using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration
{
    public class ServiceDiscoveryConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="token"></param>
        /// <param name="serviceName"></param>

        public ServiceDiscoveryConfiguration(string host, int port, string token,string serviceName)
        {
            Host = host;
            Port = port;
            Token = token; 
            ServiceName = serviceName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="token"></param>
        /// <param name="pollingInterval"></param>
        /// <param name="serviceName"></param>
        public ServiceDiscoveryConfiguration(string host, int port, string token, int pollingInterval, string serviceName)
        {
            Host = host;
            Port = port;
            Token = token; 
            PollingInterval = pollingInterval;
            ServiceName = serviceName;
        }

        public string Host { get; }

        public int Port { get; }
          
        public string Token { get; }

        public string ServiceName { get; set; }

        public int PollingInterval { get; }

        public bool Polling { get; set; }

        /// <summary>
        /// 回调函数
        /// </summary>
        public List<Action> Callback => new List<Action>();
    }
}
