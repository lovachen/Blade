using Blade.ServiceDiscovery.Providers;
using Blade.Values;
using Consul;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blade.Provider.Consul
{
    public class Consul : IServiceDiscoveryProvider
    {
        private readonly ConsulRegistryConfiguration _config;
        private readonly IConsulClient _consul;

        public Consul(ConsulRegistryConfiguration config, IConsulClientFactory clientFactory)
        { 
            _config = config; 
            _consul = clientFactory.Get(_config);
        }


        public async Task<List<Service>> Get(string servieName)
        {
            throw new NotImplementedException();
        }
    }
}
