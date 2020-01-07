using Blade.Grpc.Configuration;
using Blade.Grpc.Configuration.File;
using Blade.Grpc.Values;
using System;
using System.Collections.Concurrent;
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
        private static readonly object _syncLock = new object();
        private readonly ConcurrentDictionary<string, Lease> keyValues = new ConcurrentDictionary<string, Lease>();

        public LeastConnection(List<Service> services)
        {
            services.ForEach(service =>
            {
                var lease = new Lease(service.HostAndPort, 0);
                keyValues.AddOrUpdate(CreateKey(service.HostAndPort), lease, (k, v) => lease);
            });
        }
        public Task<ServiceHostAndPort> Lease()
        {
            if (keyValues == null || keyValues.Count == 0)
                throw new ArgumentNullException($"LeastConnection Lease serverices 为空");

            var leaseWithLeastConnections = keyValues.Values.OrderBy(x => x.Connections).First();

            leaseWithLeastConnections.Connections++;

            var hostAndPort = new ServiceHostAndPort(leaseWithLeastConnections.HostAndPort.DownstreamHost, leaseWithLeastConnections.HostAndPort.DownstreamPort);
            return Task.FromResult(hostAndPort);
        }

        public void Release(ServiceHostAndPort hostAndPort)
        {
            string key = CreateKey(hostAndPort);
            if (keyValues.TryGetValue(CreateKey(hostAndPort), out var matchingLease))
            {
                matchingLease.Connections--; 
            }
        }

        #region private


        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private string CreateKey(ServiceHostAndPort hostAndPort)
        {
            return $"{hostAndPort.DownstreamHost}:{hostAndPort.DownstreamPort}";
        }

        #endregion
    }
}
