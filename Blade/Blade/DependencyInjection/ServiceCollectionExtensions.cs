using System;
using System.Collections.Generic;
using System.Text;
using Blade.DependencyInjection;
using Blade.Grpc;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ///
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册 Blade
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IBladeBuilder AddBlade(this IServiceCollection services, IConfiguration configuration)
        { 
            return new BladeBuilder(services,configuration);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IBladeBuilder AddGrpcProfile<T>(this BladeBuilder builder) where T : GrpcProfile
        {
            builder.Services.AddSingleton<GrpcProfile, T>(); 
            return builder;
        }

    }
}
