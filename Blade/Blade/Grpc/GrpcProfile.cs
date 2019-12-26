using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc
{
    public abstract class  GrpcProfile: ConcurrentDictionary<string,string>
    { 

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <returns></returns>

        public GrpcProfile Add<T>(string serviceName)
        {
            this.AddOrUpdate(nameof(T), serviceName, (k, v) => serviceName);
            return this;
        }

    }
}
