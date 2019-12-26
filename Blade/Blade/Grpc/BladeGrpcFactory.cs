using Blade.Configuration.Create;
using Blade.Configuration.File;
using Blade.Grpc;
using Blade.LoadBalancer;
using Blade.Values;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Blade.Grpc
{
    /// <summary>
    /// 
    /// </summary>
    public class BladeGrpcFactory : IBladeGrpcFactory
    {
        private readonly ConcurrentDictionary<string, GrpcChannel> _grpcChannels;

        private IServiceProvider _serviceProvider;
        private IDownstreamProviderCreate _downstreamProviderCreate;
        private FileConfiguration _fileConfiguration;
        private ILoadBalancerHouse _loadBalancerHouse;
        private IServiceProviderConfigurationCreator _serviceProviderConfigurationCreator;

        public BladeGrpcFactory(IDownstreamProviderCreate downstreamProviderCreate,
            IOptionsMonitor<FileConfiguration> optionsMonitor,
            ILoadBalancerHouse loadBalancerHouse,
            IServiceProvider serviceProvider,
            IServiceProviderConfigurationCreator serviceProviderConfigurationCreator)
        {
            _serviceProvider = serviceProvider;
            _downstreamProviderCreate = downstreamProviderCreate;
            _serviceProviderConfigurationCreator = serviceProviderConfigurationCreator;
            _loadBalancerHouse = loadBalancerHouse;
            _fileConfiguration = optionsMonitor.CurrentValue;
            _grpcChannels = new ConcurrentDictionary<string, GrpcChannel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public async Task<GrpcChannel> Create(string serviceName)
        {
            return await Build(serviceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<GrpcChannel> Create<T>() where T : ClientBase
        {
            GrpcProfile _grpcProfile = _serviceProvider.GetService<GrpcProfile>();

            if (_grpcProfile.TryGetValue(nameof(T), out var serviceName))
            {
                return await Create(serviceName);
            }
            throw new Exception($"{nameof(T)}并未注册GrpcProfile");
        }

        #region private

        private async Task<GrpcChannel> Build(string serviceName)
        {
            GrpcChannel channel;
            var downstream = _downstreamProviderCreate.Create(_fileConfiguration, serviceName);
            var serviceProvider = _serviceProviderConfigurationCreator.Create(_fileConfiguration.GlobalConfiguration);
            var file = _fileConfiguration.GlobalConfiguration.ServiceDiscoveryProvider;

            var loadBalancer = await _loadBalancerHouse.Get(downstream, serviceProvider);
            var hostAndPort = await loadBalancer.Lease(serviceProvider);
            var key = CreateKey(hostAndPort, serviceName);
            if (_grpcChannels.TryGetValue(key, out var _channel))
            {
                channel = _grpcChannels[key];
            }
            else
            { 
                channel = CreateChannel(hostAndPort);
                _grpcChannels[key] = channel;
            }
            return channel;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadBalancer"></param>
        /// <returns></returns>
        private string CreateKey(ServiceHostAndPort hostAndPort, string serviceName)
        {
            return $"{serviceName}|{hostAndPort.DownstreamHost}:{hostAndPort.DownstreamPort}";
        }

        private GrpcChannel CreateChannel(ServiceHostAndPort hostAndPort)
        {
           return GrpcChannel.ForAddress($"http://{hostAndPort.DownstreamHost}:{hostAndPort.DownstreamPort}");
        }

        #endregion
    }
}
