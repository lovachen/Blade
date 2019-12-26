using Blade.Configuration.File;
using System.Collections.Generic;

namespace Blade.Configuration.Create
{
    public interface IDownstreamProviderCreate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        List<DownstreamProvider> Create(FileConfiguration config);

        /// <summary>
        /// 构建对象
        /// </summary>
        /// <param name="config"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        DownstreamProvider Create(FileConfiguration config, string serviceName);
    }
}