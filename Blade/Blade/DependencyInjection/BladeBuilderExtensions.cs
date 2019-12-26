using Blade.Grpc;
using Microsoft.Extensions.DependencyInjection;

namespace Blade.DependencyInjection
{
    public static class BladeBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IBladeBuilder AddGrpcProfile<T>(this IBladeBuilder builder) where T : GrpcProfile
        {
            builder.Services.AddSingleton<GrpcProfile, T>();
            return builder;
        }
    }
}
