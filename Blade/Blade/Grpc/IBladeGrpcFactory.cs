﻿using Blade.Grpc.Values;
using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace Blade.Grpc
{
    public interface IBladeGrpcFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task<GrpcChannel> Create(string serviceName);

        /// <summary>
        /// 通过 类型创建连接通道 时 需要先配置 GrpcProfile
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<GrpcChannel> Create<T>() where T : ClientBase;
         
    }
}