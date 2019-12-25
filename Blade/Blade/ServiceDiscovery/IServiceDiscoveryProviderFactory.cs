using Blade.Configuration;
using Blade.ServiceDiscovery.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.ServiceDiscovery
{
    public interface IServiceDiscoveryProviderFactory
    {
        IServiceDiscoveryProvider Get(ServiceProviderConfiguration serviceConfig, DownstreamProvider downstream);

    }
}
