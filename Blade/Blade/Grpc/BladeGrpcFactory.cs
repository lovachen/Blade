using Grpc.Blade.Configuration.Create;
using Grpc.Blade.LoadBalancer;
using Grpc.Blade.Profile;
using Grpc.Blade.Values;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grpc.Blade
{
    /// <summary>
    /// 
    /// </summary>
    public class BladeGrpcFactory : IBladeGrpcFactory
    {
        private readonly ConcurrentDictionary<string, GrpcChannel> _grpcChannels;

        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInternalConfigurationCreate _internalConfigurationCreate;
        private readonly ILoadBalancerHouse _loadBalancerHouse;
        private readonly IServiceProviderConfigurationCreator _serviceProviderConfigurationCreator;
        private readonly IDownstreamProviderCreate _downstreamProviderCreate;

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostAndPort"></param>
        /// <returns></returns>
        public async Task Remove(ServiceHostAndPort hostAndPort)
        {
            string key = CreateKey(hostAndPort);
            _grpcChannels.TryRemove(key, out var _);
            await Task.CompletedTask;
        }

        #region private

        private async Task<GrpcChannel> Build(string serviceName)
        {
            var _fileConfiguration = _internalConfigurationCreate.Get();

            var downstream = _downstreamProviderCreate.Create(_fileConfiguration, serviceName);

            var serviceProvider = _serviceProviderConfigurationCreator.Create(_fileConfiguration.GlobalConfiguration);

            var loadBalancer = await _loadBalancerHouse.Get(downstream, serviceProvider);
            var hostAndPort = await loadBalancer.Lease(serviceProvider);

            var channel = CreateChannel(hostAndPort);
            UpdateItems(loadBalancer, hostAndPort);
            return channel;
        }

        private GrpcChannel CreateChannel(ServiceHostAndPort hostAndPort)
        {
            string key = CreateKey(hostAndPort);

            if (_grpcChannels.TryGetValue(key, out GrpcChannel _channel))
            {
                return _channel;
            }
            else
            {
                var channel = GrpcChannel.ForAddress($"http://{hostAndPort.DownstreamHost}:{hostAndPort.DownstreamPort}");
                _grpcChannels.AddOrUpdate(key, channel, (k, v) => channel);
                return channel;
            }
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
