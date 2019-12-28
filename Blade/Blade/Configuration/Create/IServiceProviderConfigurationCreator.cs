using Grpc.Blade.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grpc.Blade.Configuration.Create
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
