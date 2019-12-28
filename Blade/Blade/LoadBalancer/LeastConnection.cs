using Blade.Grpc.Configuration;
using Blade.Grpc.Configuration.File;
using Blade.Grpc.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Grpc.LoadBalancer
{
    /// <summary>
    /// 最小连接负载均衡
    /// </summary>
    public class LeastConnection : ILoadBalancer
    {
        private readonly List<Service> _services;
        private readonly List<Lease> _leases; 
        private static readonly object _syncLock = new object();


        public LeastConnection(List<Service> services)
        {
            _services = services;
            _leases = new List<Lease>();
        }
        public Task<ServiceHostAndPort> Lease(ServiceProviderConfiguration config)
        {
            if (_services == null || _services.Count == 0)
                throw new ArgumentNullException($"LeastConnection Lease serverices 为空");

            lock (_syncLock)
            {
                UpdateServices(_services);
                var leaseWithLeastConnections = GetLeaseWithLeastConnections();
                _leases.Remove(leaseWithLeastConnections);

                leaseWithLeastConnections = AddConnection(leaseWithLeastConnections);

                _leases.Add(leaseWithLeastConnections);
                var hostAndPort = new ServiceHostAndPort(leaseWithLeastConnections.HostAndPort.DownstreamHost, leaseWithLeastConnections.HostAndPort.DownstreamPort);

                return Task.FromResult(hostAndPort);
            }
        }

        public void Release(ServiceHostAndPort hostAndPort)
        {
            lock (_syncLock)
            {
                var matchingLease = _leases.FirstOrDefault(l => l.HostAndPort.DownstreamHost == hostAndPort.DownstreamHost
                    && l.HostAndPort.DownstreamPort == hostAndPort.DownstreamPort);

                if (matchingLease != null)
                {
                    var replacementLease = new Lease(hostAndPort, matchingLease.Connections - 1);

                    _leases.Remove(matchingLease);

                    _leases.Add(replacementLease);
                }
            }
        }

        #region private

        public void UpdateServices(List<Service> services)
        {
            if (_leases.Count == 0)
            {
                var leasesToRemove = new List<Lease>();

                foreach (var lease in _leases)
                {
                    var match = _services.FirstOrDefault(s => s.HostAndPort.DownstreamHost == lease.HostAndPort.DownstreamHost
                   && s.HostAndPort.DownstreamPort == lease.HostAndPort.DownstreamPort);
                    if (match == null)
                    {
                        leasesToRemove.Add(lease);
                    }
                }
                foreach (var lease in leasesToRemove)
                {
                    _leases.Remove(lease);
                }

                foreach (var service in _services)
                {
                    var exists = _leases.FirstOrDefault(s => s.HostAndPort.DownstreamHost == service.HostAndPort.DownstreamHost
                     && s.HostAndPort.DownstreamPort == service.HostAndPort.DownstreamPort);
                    if (exists == null)
                    {
                        _leases.Add(new Lease(service.HostAndPort, 0));
                    }
                }
            }
            else
            {
                foreach (var service in services)
                {
                    _leases.Add(new Lease(service.HostAndPort, 0));
                }
            }
        }

        private Lease GetLeaseWithLeastConnections()
        {
            Lease leaseWithLeastConnections = null;

            for (var i = 0; i < _leases.Count; i++)
            {
                if (i == 0)
                {
                    leaseWithLeastConnections = _leases[i];
                }
                else
                {
                    if (_leases[i].Connections < leaseWithLeastConnections.Connections)
                    {
                        leaseWithLeastConnections = _leases[i];
                    }
                }
            }
            return leaseWithLeastConnections;
        }

        private Lease AddConnection(Lease lease)
        {
            return new Lease(lease.HostAndPort, lease.Connections + 1);
        }

        #endregion
    }
}
