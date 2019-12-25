using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Values
{
    /// <summary>
    /// 服务地址
    /// </summary>
    public class ServiceHostAndPort
    {
        public ServiceHostAndPort(string downstreamHost, int downstreamPort)
        {
            DownstreamHost = downstreamHost?.Trim('/');
            DownstreamPort = downstreamPort;
        }

        public string DownstreamHost { get; }

        public int DownstreamPort { get; }
    }
}
