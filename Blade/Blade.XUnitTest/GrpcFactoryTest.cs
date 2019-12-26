using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blade.XUnitTest
{
   public class GrpcFactoryTest:TestBase
    {

        public GrpcFactoryTest(ITestOutputHelper testOutput)
        {
            OutputHelper = testOutput;
        }

        [Fact]
        public void Get_GrpcChanel()
        {
           var grpcFactory = ServiceProvider.GetService<Grpc.IBladeGrpcFactory>();

           var channel = grpcFactory.Create("grpc.user");

            Assert.NotNull(channel);

        }

    }
}
