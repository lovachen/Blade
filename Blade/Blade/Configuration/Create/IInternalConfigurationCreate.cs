using Blade.Configuration.File;

namespace Blade.Configuration.Create
{
    public interface IInternalConfigurationCreate
    {
        InternalConfiguration Get();

        void AddOrReplace(InternalConfiguration internalConfiguration);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileConfiguration"></param>
        void AddOrReplace(FileConfiguration fileConfiguration);

    }
}