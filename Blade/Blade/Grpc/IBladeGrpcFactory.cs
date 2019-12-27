﻿using Grpc.Core;
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<GrpcChannel> Create<T>() where T : ClientBase;
    }
}