using Grpc.Blade.Configuration;
using Grpc.Blade.Provider.Consul.Failover;
using Grpc.Blade.Provider.Consul.Util;
using Grpc.Blade.ServiceDiscovery.Providers;
using Grpc.Blade.Values;
using Consul;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Grpc.Blade.Provider.Consul
{
    public class Consul : IServiceDiscoveryProvider, IDisposable
    {
        private readonly static Dictionary<string, Timer> listeners = new Dictionary<string, Timer>();
        private readonly IConsulClientFactory _factory;
        private readonly ILocalProcessor _localProcessor;
        private const string VersionPrefix = "version-";
        private readonly ILogger _logger;

        public Consul(IConsulClientFactory clientFactory,
            ILoggerFactory loggerFactory,
            ILocalProcessor localProcessor)
        {
            _logger = loggerFactory.CreateLogger<Consul>();
            _localProcessor = localProcessor;
            _factory = clientFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Task<List<Service>> GetServices(ServiceDiscoveryConfiguration config)
        {
            var cfg = new ConsulRegistryConfiguration(config.Host, config.Port, config.ServiceName, config.Token);
            return GetServices(_factory.Get(cfg), cfg);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public void AddListener(ServiceDiscoveryConfiguration config)
        {
            string key = CreateKey(config);

            //如果已添加过监听
            if (listeners.ContainsKey(key))
            {
                return;
            }
            Timer timer = new Timer(async o =>
            {
                var cfg = (ServiceDiscoveryConfiguration)o;
                if (cfg.Polling) return;
                cfg.Polling = true;
                await PollingAsync(o);
                cfg.Polling = false;
            }, config, 0, config.PollingInterval);

        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            foreach (var timer in listeners)
                timer.Value?.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void ClearListner()
        {
            foreach (var timer in listeners)
                timer.Value?.Dispose();
            listeners.Clear();
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public void RemoveListner(ServiceDiscoveryConfiguration config)
        {
            string key = CreateKey(config);

            if (listeners.ContainsKey(key))
            {
                listeners[key]?.Dispose();
            }
        }

        #region private

        private bool IsValid(ServiceEntry serviceEntry)
        {
            if (String.IsNullOrEmpty(serviceEntry.Service.Address) || serviceEntry.Service.Address.Contains("http://") || serviceEntry.Service.Address.Contains("https://") || serviceEntry.Service.Port <= 0)
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

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        private async Task<List<Service>> GetServices(IConsulClient consul, ConsulRegistryConfiguration config)
        {
            var services = await _localProcessor.GetConfigAsync(config.Host, config.Port, config.KeyOfServiceInConsul);

            if (services != null && services.Any())
            {
                return services;
            }
            services = await ClientGetServices(consul, config);
            if (services != null && services.Any())
            {
                await _localProcessor.SaveConfigAsync(config.Host, config.Port, config.KeyOfServiceInConsul, services);
            }

            return services;
        }

        /// <summary>
        /// 轮询请求获取Consul服务
        /// MD5校验如果更改则通知回调
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private async Task PollingAsync(object info)
        {
            var cfg = (ServiceDiscoveryConfiguration)info;
            var config = new ConsulRegistryConfiguration(cfg.Host, cfg.Port, cfg.ServiceName, cfg.Token);
            var consul = _factory.Get(config);

            var localServices = await _localProcessor.GetConfigAsync(config.Host, config.Port, config.KeyOfServiceInConsul);

            var services = await ClientGetServices(consul, config);
            if (!Compare(localServices, services))
            {
                try
                {
                    if (cfg.Callback != null)
                        cfg.Callback.ForEach(ac=>ac());
                }
                catch (Exception ex)
                {
                    _logger.LogError($"轮询Consul通知回调错误，host:{cfg.Host},port:{cfg.Port},servicename:{cfg.ServiceName}描述:{ex.Message}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consul"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private async Task<List<Service>> ClientGetServices(IConsulClient consul, ConsulRegistryConfiguration config)
        {
            try
            {
                var services = new List<Service>();
                var queryResult = await consul.Health.Service(config.KeyOfServiceInConsul, string.Empty, true);

                foreach (var serviceEntry in queryResult.Response)
                {
                    if (IsValid(serviceEntry))
                    {
                        services.Add(BuildService(serviceEntry));
                    }
                }
                return services;
            }
            catch (Exception ex)
            {
                _logger.LogError($"请求Consul 获取服务时错误，host:{config.Host},port:{config.Port},servicename:{config.KeyOfServiceInConsul}描述:{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 验证结果是否一致
        /// </summary>
        /// <param name="local"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        private bool Compare(List<Service> local, List<Service> services)
        {
            if (local == null || services == null) return false;

            return MD5Util.GetMD5(JsonSerializer.Serialize(local))
                 .Equals(MD5Util.GetMD5(JsonSerializer.Serialize(services)));
        }

        private string CreateKey(ServiceDiscoveryConfiguration config)
        {
            return $"{config.Host}-{config.Port}-{config.ServiceName}";
        }



        #endregion
    }
}
