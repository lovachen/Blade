using Blade.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.Create
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
