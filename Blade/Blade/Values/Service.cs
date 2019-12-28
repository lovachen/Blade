﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Grpc.Values
{
    /// <summary>
    /// 服务
    /// </summary>
    public class Service
    {
        public Service(string name,
            ServiceHostAndPort hostAndPort,
            string id,
            string version,
            IEnumerable<string> tags)
        {
            Name = name;
            HostAndPort = hostAndPort;
            Id = id;
            Version = version;
            Tags = tags;
        }

        public string Id { get; }

        public string Name { get; }

        public string Version { get; }

        public IEnumerable<string> Tags { get; }

        public ServiceHostAndPort HostAndPort { get; }
    }
}
