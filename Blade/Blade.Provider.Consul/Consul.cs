using Blade.ServiceDiscovery.Providers;
using Blade.Values;
using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blade.Provider.Consul
{
    public class Consul : IServiceDiscoveryProvider
    {
        private readonly ConsulRegistryConfiguration _config;
        private readonly IConsulClient _consul;
        private const string VersionPrefix = "version-";

        public Consul(ConsulRegistryConfiguration config, IConsulClientFactory clientFactory)
        {
            _config = config;
            _consul = clientFactory.Get(_config);
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public async Task<List<Service>> GetServices()
        {
            var services = new List<Service>();
            var queryResult = await _consul.Health.Service(_config.KeyOfServiceInConsul, string.Empty, true);

            foreach (var serviceEntry in queryResult.Response)
            {
                if (IsValid(serviceEntry))
                {
                    services.Add(BuildService(serviceEntry));
                }
            }
            return services;
        }

        #region private

        private bool IsValid(ServiceEntry serviceEntry)
        {
            if (string.IsNullOrEmpty(serviceEntry.Service.Address) || serviceEntry.Service.Address.Contains("http://") || serviceEntry.Service.Address.Contains("https://") || serviceEntry.Service.Port <= 0)
            {
                return false;
            }

            return true;
        }
        private Service BuildService(ServiceEntry serviceEntry)
        {
            return new Service(
                serviceEntry.Service.Service,
                new ServiceHostAndPort(serviceEntry.Service.Address, serviceEntry.Service.Port),
                serviceEntry.Service.ID,
                GetVersionFromStrings(serviceEntry.Service.Tags),
                serviceEntry.Service.Tags ?? Enumerable.Empty<string>());
        }

        private string GetVersionFromStrings(IEnumerable<string> strings)
        {
            var versions = strings
                ?.FirstOrDefault(x => x.StartsWith(VersionPrefix, StringComparison.Ordinal));
            return versions?.Substring(VersionPrefix.Length);
        }

        #endregion
    }
}
