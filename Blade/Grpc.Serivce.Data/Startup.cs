using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Grpc.Serivce.Data.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Grpc.Serivce.Data
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 服务注册基础信息配置
            services.Configure<ServiceRegisterOptions>(Configuration.GetSection("ServiceRegister"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceRegisterOptions>>().Value;
                if (!String.IsNullOrEmpty(serviceConfiguration.Register.HttpEndpoint))
                {
                    cfg.Address = new Uri(serviceConfiguration.Register.HttpEndpoint);
                }
            }));
            #endregion


            services.AddGrpc(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<ServiceRegisterOptions> serviceRegisterOptions,
            IConsulClient consul, IHostApplicationLifetime appLife)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<UserServices>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });

                //consul心跳健康检查地址
                endpoints.MapGet("/api/health", async context =>
                 {
                     await context.Response.WriteAsync("ok");
                 });
            });


            var serviceId = $"{serviceRegisterOptions.Value.ServiceName}_{serviceRegisterOptions.Value.ServiceHost}:{serviceRegisterOptions.Value.ServicePort}";

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(1),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(2),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{serviceRegisterOptions.Value.ServiceHost}:{serviceRegisterOptions.Value.ServicePort}/api/health",//健康检查地址
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                Address = serviceRegisterOptions.Value.ServiceHost,
                ID = serviceId,
                Name = serviceRegisterOptions.Value.ServiceName,
                Port = serviceRegisterOptions.Value.ServicePort 
            }; 
            consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            appLife.ApplicationStopping.Register(() =>
            {
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();//服务停止时取消注册
            });
             
        }
    }
}
