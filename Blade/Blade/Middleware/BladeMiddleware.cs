using Grpc.Blade;
using Grpc.Blade.LoadBalancer;
using Grpc.Blade.Values;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grpc.Blade.Middleware
{
    public class BladeMiddleware
    {
        private readonly RequestDelegate _next; 

        public BladeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Controller action调用结束后归还
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);

            context.Response.OnCompleted(async () =>
            {
                if (context.Items.ContainsKey(ConstantValue.CHANNEL_ITEMS))
                {
                    var currents = context.Items[ConstantValue.CHANNEL_ITEMS] as Dictionary<string,LoadBalancerHttpItems>;
                    foreach (var item in currents)
                    {
                        item.Value.LoadBalancer.Release(item.Value.ServiceHostAndPort);
                    }
                }
                await Task.FromResult(0);
            });
        }
    }
}
