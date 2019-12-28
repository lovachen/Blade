namespace Grpc.Blade.DependencyInjection
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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

    }
}
