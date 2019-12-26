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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollingInterval"></param>
        /// <param name="consulServiceDiscoveryProvider"></param>
        public PollConsul(int pollingInterval, IServiceDiscoveryProvider consulServiceDiscoveryProvider)
        {
            _consulServiceDiscoveryProvider = consulServiceDiscoveryProvider;
            _services = new List<Service>();
            _timer = new Timer(async x =>
              {
                  if (_polling) return;
                  _polling = true;
                  await Poll();
                  _polling = false;
              }, null, pollingInterval, pollingInterval);

        }
        public void Dispose()
        {
            _timer?.Dispose();
            _timer = null;
        }

        public Task<List<Service>> GetServices()
        {
            return Task.FromResult(_services);
        }

        private async Task Poll()
        {
            _services = await _consulServiceDiscoveryProvider.GetServices();
        }
    }
}
