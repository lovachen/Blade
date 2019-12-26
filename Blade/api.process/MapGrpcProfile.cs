﻿using Blade.Grpc;
using grpc.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.process
{
    public class MapGrpcProfile: GrpcProfile
    {
        public MapGrpcProfile()
        {
            Add<User.UserClient>("grpc.user"); 
            Add<Greeter.GreeterClient>("grpc.user"); 
        }
    }
}
