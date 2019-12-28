using Blade.Grpc.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class InternalConfiguration
    {
        public InternalConfiguration()
        {
            GlobalConfiguration = new FileGlobalConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        public FileGlobalConfiguration GlobalConfiguration { get; set; }
    }
}
