using Blade.Grpc.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration.Create
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceProviderConfigurationCreator : IServiceProviderConfigurationCreator
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="globalConfiguration"></param>
        /// <returns></returns>
        public ServiceProviderConfiguration Create(FileGlobalConfiguration globalConfiguration)
        {
            return Build(globalConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="globalConfiguration"></param>
        /// <returns></returns>
        private ServiceProviderConfiguration Build(FileGlobalConfiguration globalConfiguration)
        {
            var port = globalConfiguration?.ServiceDiscoveryProvider?.Port ?? 0;
            var host = globalConfiguration?.ServiceDiscoveryProvider?.Host ?? "localhost";
            var token = globalConfiguration?.ServiceDiscoveryProvider?.Token;

            return new ServiceProviderConfiguration(host, port, token);
        }
    }
}
