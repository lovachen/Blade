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
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Blade.Grpc
{
    /// <summary>
    /// 
    /// </summary>
    public class BladeGrpcFactory : IBladeGrpcFactory
    {
        private readonly ConcurrentDictionary<string, GrpcChannel> _grpcChannels;

        private IServiceProvider _serviceProvider;
        private IHttpContextAccessor _httpContextAccessor;
        private IDownstreamProviderCreate _downstreamProviderCreate;
        private IInternalConfigurationCreate _internalConfigurationCreate;
        private ILoadBalancerHouse _loadBalancerHouse;
        private IServiceProviderConfigurationCreator _serviceProviderConfigurationCreator;

        public BladeGrpcFactory(IDownstreamProviderCreate downstreamProviderCreate,
            IInternalConfigurationCreate internalConfigurationCreate,
            ILoadBalancerHouse loadBalancerHouse,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor,
            IServiceProviderConfigurationCreator serviceProviderConfigurationCreator)
        {
            _serviceProvider = serviceProvider;
            _downstreamProviderCreate = downstreamProviderCreate;
            _serviceProviderConfigurationCreator = serviceProviderConfigurationCreator;
            _loadBalancerHouse = loadBalancerHouse;
            _internalConfigurationCreate = internalConfigurationCreate;
            _httpContextAccessor = httpContextAccessor;
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
            if (_grpcProfile.TryGetValue(typeof(T).FullName, out var serviceName))
            {
                return await Create(serviceName);
            }
            throw new Exception($"{nameof(T)}并未注册GrpcProfile");
        }

        #region private

        private async Task<GrpcChannel> Build(string serviceName)
        {
            var _fileConfiguration = _internalConfigurationCreate.Get();
            var downstream = _downstreamProviderCreate.Create(_fileConfiguration, serviceName);
            var serviceProvider = _serviceProviderConfigurationCreator.Create(_fileConfiguration.GlobalConfiguration);
            var file = _fileConfiguration.GlobalConfiguration.ServiceDiscoveryProvider;
            var loadBalancer = await _loadBalancerHouse.Get(downstream, serviceProvider);
            var hostAndPort = await loadBalancer.Lease(serviceProvider);
            var channel = CreateChannel(hostAndPort);
            UpdateItems(loadBalancer, hostAndPort);
            return channel;
        }

        private GrpcChannel CreateChannel(ServiceHostAndPort hostAndPort)
        {
            return GrpcChannel.ForAddress($"http://{hostAndPort.DownstreamHost}:{hostAndPort.DownstreamPort}");
        }

        private void UpdateItems(ILoadBalancer loadBalancer, ServiceHostAndPort hostAndPort)
        {
            Dictionary<string, LoadBalancerHttpItems> dic;
            string key = CreateKey(hostAndPort);
            if (_httpContextAccessor.HttpContext.Items.ContainsKey(ConstantValue.CHANNEL_ITEMS))
            {
                dic = _httpContextAccessor.HttpContext.Items[ConstantValue.CHANNEL_ITEMS] as Dictionary<string, LoadBalancerHttpItems>;
            }
            else
            {
                dic = new Dictionary<string, LoadBalancerHttpItems>();
                _httpContextAccessor.HttpContext.Items[ConstantValue.CHANNEL_ITEMS] = dic;
            }
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, new LoadBalancerHttpItems(hostAndPort, loadBalancer));
            }
        }

        private string CreateKey(ServiceHostAndPort hostAndPort)
        {
            return $"{hostAndPort.DownstreamHost}:{hostAndPort.DownstreamPort}";
        }


        #endregion
    }

}
