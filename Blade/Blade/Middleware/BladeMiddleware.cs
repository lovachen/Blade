﻿using Blade.Grpc;
using Blade.LoadBalancer;
using Blade.Values;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Middleware
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
                    var currents = context.Items[ConstantValue.CHANNEL_ITEMS] as List<ServiceHostAndPort>;
                    foreach (var servicesHost in currents)
                    {

                    }
                }
                await Task.FromResult(0);
            });
        }
    }
}
