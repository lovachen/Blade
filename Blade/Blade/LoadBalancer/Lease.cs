﻿using Blade.Grpc.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.LoadBalancer
{
    /// <summary>
    /// 
    /// </summary>
    internal class Lease
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostAndPort"></param>
        /// <param name="connections"></param>
        public Lease(ServiceHostAndPort hostAndPort, int connections)
        {
            HostAndPort = hostAndPort;
            Connections = connections;
        }

        /// <summary>
        /// 
        /// </summary>
        public ServiceHostAndPort HostAndPort { get; private set; }

        /// <summary>
        /// 连接数
        /// </summary>
        public int Connections { get; internal set; }
    }
}
