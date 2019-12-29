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
            BladeGrpc = new FileGlobalConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        public FileGlobalConfiguration BladeGrpc { get; set; }

    }
}
