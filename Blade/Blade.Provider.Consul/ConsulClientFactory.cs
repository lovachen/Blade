using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Provider.Consul
{
    public class ConsulClientFactory : IConsulClientFactory
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IConsulClient Get(ConsulRegistryConfiguration config)
        {
            return new ConsulClient(c =>
            {
                c.Address = new Uri($"http://{config.Host}:{config.Port}");

                if (!String.IsNullOrEmpty(config?.Token))
                {
                    c.Token = config.Token;
                }
            });
        }
    }
}
