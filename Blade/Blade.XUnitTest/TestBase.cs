using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.XUnitTest
{
    public class TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider;

        public TestBase()
        {
            IServiceCollection services = new ServiceCollection();






            ServiceProvider = services.BuildServiceProvider();
        }

    }
}
