using Blade.ServiceDiscovery.Providers;
using Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blade.Provider.Consul
{
    public class PollConsul : IServiceDiscoveryProvider, IDisposable
    {
        private readonly IServiceDiscoveryProvider _consulServiceDiscoveryProvider;
        private Timer _timer;
        private bool _polling;
        private List<Service> _services;

        public PollConsul(int pollingInterval, IServiceDiscoveryProvider consulServiceDiscoveryProvider)
        {



        }
        public void Dispose()
        {
        } 

        public Task<List<Service>> Get()
        {
            throw new NotImplementedException();
        }
    }
}
