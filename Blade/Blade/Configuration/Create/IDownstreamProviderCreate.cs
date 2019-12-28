using Blade.Configuration.File;
using System.Collections.Generic;

namespace Blade.Configuration.Create
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDownstreamProviderCreate
    {
        /// <summary>
        /// 构建下游对象列表
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        List<DownstreamProvider> Create(InternalConfiguration config);

        /// <summary>
        /// 构建对象
        /// </summary>
        /// <param name="config"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        DownstreamProvider Create(InternalConfiguration config, string serviceName);
    }
}