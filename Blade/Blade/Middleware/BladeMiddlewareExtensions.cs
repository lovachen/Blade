using Blade.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Blade.Configuration.File;
using Blade.Configuration.Create;

namespace Microsoft.AspNetCore.Builder
{
    public static class BladeMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseBlade(this IApplicationBuilder app)
        {
            CreateConfiguration(app);
            app.UseMiddleware<BladeMiddleware>();
            return app;
        }


        /// <summary>
        /// 监听json配置文件更改
        /// </summary>
        /// <param name="builder"></param>
        private static void CreateConfiguration(IApplicationBuilder builder)
        {
           var configurationCreate = builder.ApplicationServices.GetService<IInternalConfigurationCreate>();
            var fileConfig = builder.ApplicationServices.GetService<IOptionsMonitor<FileConfiguration>>();

            configurationCreate.AddOrReplace(fileConfig.CurrentValue);

            fileConfig.OnChange(config =>
            {
                configurationCreate.AddOrReplace(config);
            });

        }


    }
}
