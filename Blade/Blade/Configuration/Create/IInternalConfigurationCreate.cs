using Blade.Configuration.File;

namespace Blade.Configuration.Create
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInternalConfigurationCreate
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        InternalConfiguration Get();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="internalConfiguration"></param>
        void AddOrReplace(InternalConfiguration internalConfiguration);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fileConfiguration"></param>
        void AddOrReplace(FileConfiguration fileConfiguration);

    }
}