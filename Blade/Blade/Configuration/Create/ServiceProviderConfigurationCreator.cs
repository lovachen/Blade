﻿using Blade.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.Create
{
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


        private ServiceProviderConfiguration Build(FileGlobalConfiguration globalConfiguration)
        {
            var port = globalConfiguration?.ServiceDiscoveryProvider?.Port ?? 0;
            var host = globalConfiguration?.ServiceDiscoveryProvider?.Host ?? "localhost";
            var token = globalConfiguration?.ServiceDiscoveryProvider?.Token;

            return new ServiceProviderConfiguration(host, port, token);
        }
    }
}
