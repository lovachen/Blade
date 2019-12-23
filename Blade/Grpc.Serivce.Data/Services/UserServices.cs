using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grpc.Serivce.Data
{
    public class UserServices : User.UserBase
    {


        public override Task<UserReply> GetUser(UserRequest request, ServerCallContext context)
        {
            string ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
              .Select(p => p.GetIPProperties())
              .SelectMany(p => p.UnicastAddresses)
              .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
              .FirstOrDefault()?.Address.ToString();

            return Task.FromResult(new UserReply()
            {
                Name = context.Host,
                Mobile = ip
            });
        }
    }
}
