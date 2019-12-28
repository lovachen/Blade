using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grpc.Blade.DependencyInjection
{
    public interface IBladeBuilder
    {

        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }
    }
}
