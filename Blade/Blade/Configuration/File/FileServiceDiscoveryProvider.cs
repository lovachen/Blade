using System;
using System.Collections.Generic;
using System.Text;

namespace Grpc.Blade.Configuration.File
{
    /// <summary>
    /// 
    /// </summary>
    public class FileServiceDiscoveryProvider
    { 
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public int PollingInterval { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public bool Listening { get; set; }

    }
}
