using Blade.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.LoadBalancer
{
    public class Lease
    {
        public Lease(ServiceHostAndPort hostAndPort, int connections)
        {
            HostAndPort = hostAndPort;
            Connections = connections;
        }

        public ServiceHostAndPort HostAndPort { get; private set; }
        public int Connections { get; private set; }
    }
}
