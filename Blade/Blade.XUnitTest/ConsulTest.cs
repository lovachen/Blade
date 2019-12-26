using Blade.Provider.Consul;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Blade.ServiceDiscovery.Providers;
using Xunit.Abstractions;

namespace Blade.XUnitTest
{
    public class ConsulTest:TestBase
    {
        public ConsulTest(ITestOutputHelper testOutput)
        {
            OutputHelper = testOutput;
        }

        [Fact]
        public void Get_ConsulServices()
        {
           var consulClientFactory =  ServiceProvider.GetService<IConsulClientFactory>();
           var consul = ServiceProvider.GetService<IServiceDiscoveryProvider>(); 

            var res =  consul.GetServices().Result;
            Assert.NotNull(res);
            foreach (var item in res)
            {
                OutputHelper.WriteLine($"{item.HostAndPort.DownstreamHost}:{item.HostAndPort.DownstreamPort}");
            } 
             

        }



    }
}
