using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration.File
{
    /// <summary>
    /// 
    /// </summary>
    public class FileConfiguration
    {
        public FileConfiguration()
        {
            GlobalConfiguration = new FileGlobalConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        public FileGlobalConfiguration GlobalConfiguration { get; set; }

    }
}
