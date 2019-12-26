using Blade.Configuration;
using Blade.Configuration.File;
using Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    public class LeastConnection : ILoadBalancer
    {
        private List<Service> _services;


        public LeastConnection(List<Service> services)
        {
            _services = services;
        }
        public Task<ServiceHostAndPort> Lease(ServiceProviderConfiguration config)
        {
            throw new NotImplementedException();
        }

        public void Release(ServiceHostAndPort hostAndPort)
        {
            throw new NotImplementedException();
        }
    }
}
