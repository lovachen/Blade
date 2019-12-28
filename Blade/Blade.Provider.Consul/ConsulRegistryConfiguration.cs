using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Provider.Consul
{
    /// <summary>
    /// Consul 注册配置
    /// </summary>
   public class ConsulRegistryConfiguration
    {
        public ConsulRegistryConfiguration(string host, int port, string keyOfServiceInConsul, string token)
        {
            Host = string.IsNullOrEmpty(host) ? "localhost" : host;
            Port = port > 0 ? port : 8500;
            KeyOfServiceInConsul = keyOfServiceInConsul;
            Token = token;
        }

        public string KeyOfServiceInConsul { get; }
        public string Host { get; }
        public int Port { get; }
        public string Token { get; }
    }
}
