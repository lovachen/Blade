using Blade.Configuration.File;
using Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.LoadBalancer
{
    public class NoLoadBalancer : ILoadBalancer
    {
        private List<Service> _services;


        public NoLoadBalancer(List<Service> services)
        {
            _services = services;
        }

        public Task<ServiceHostAndPort> Lease(FileServiceDiscoveryProvider provider)
        {
            throw new NotImplementedException();
        }

        public void Release(ServiceHostAndPort hostAndPort)
        {
            throw new NotImplementedException();
        }
    }
}
