using Grpc.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Blade.Grpc.Profile
{
    /// <summary>
    /// 继承此类 并启用 AddGrpcProfile
    /// </summary>
    public abstract class  GrpcProfile: Dictionary<string,string>
    {  
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <returns></returns>

        public void Add<T>(string serviceName) where T :ClientBase
        {
            this.TryAdd(typeof(T).FullName, serviceName);
        }

    }
}
