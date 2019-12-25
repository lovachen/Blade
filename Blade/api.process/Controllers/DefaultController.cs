using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using grpc.user;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.process.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("say")]
        public IActionResult Say()
        {
            //var httpClientHandler = new HttpClientHandler();
            //// Return `true` to allow certificates that are untrusted/invalid
            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //var httpClient = new HttpClient(httpClientHandler);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //var channel = GrpcChannel.ForAddress("http://192.168.0.121:7001/", new GrpcChannelOptions { HttpClient = httpClient });
            var channel = GrpcChannel.ForAddress("http://192.168.0.105:7001/");

            var greeterClient = new Greeter.GreeterClient(channel);
            var res = greeterClient.SayHello(new HelloRequest()
            {
                Name = "李斯"
            }); ;
            var userClient = new User.UserClient(channel);
            var res2 = userClient.GetUser(new UserRequest()
            {
                Id = "1"
            }); ;
            return Ok(new { success = true, data = res, dta = res2 });
        }

    }
}