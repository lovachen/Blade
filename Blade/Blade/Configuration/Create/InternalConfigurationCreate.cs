using Blade.Grpc.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration.Create
{
    /// <summary>
    /// 
    /// </summary>
    public class InternalConfigurationCreate : IInternalConfigurationCreate
    {
        private static readonly object LockObject = new object();

        private InternalConfiguration _internalConfiguration;

        public void AddOrReplace(InternalConfiguration internalConfiguration)
        {
            lock (LockObject)
            {
                _internalConfiguration = internalConfiguration;
            }
        }

        ///
        public void AddOrReplace(FileConfiguration fileConfiguration)
        {
            InternalConfiguration internalConfiguration = new InternalConfiguration
            {
                GlobalConfiguration = fileConfiguration.BladeGrpc
            };
            AddOrReplace(internalConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public InternalConfiguration Get()
        {
            return _internalConfiguration;
        }




    }
}
