using Blade.Grpc.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration.Create
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceProviderConfigurationCreator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="globalConfiguration"></param>
        /// <returns></returns>
        ServiceProviderConfiguration Create(FileGlobalConfiguration globalConfiguration);
    }
}
