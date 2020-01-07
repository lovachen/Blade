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
    public class NoLoadBalancer : ILoadBalancer
    {
        private readonly List<Service> _services;


        public NoLoadBalancer(List<Service> services)
        {
            _services = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<ServiceHostAndPort> Lease()
        {
            if (_services == null || _services.Count == 0)
                throw new Exception($"Lease NoLoadBalancer 构建时 List<Service> 不存在值");
             
            return await Task.FromResult(_services.FirstOrDefault().HostAndPort);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostAndPort"></param>
        public void Release(ServiceHostAndPort hostAndPort)
        {

        }
    }
}
