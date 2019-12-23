using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Serivce.Data;
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
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var greeterClient = new Greeter.GreeterClient(channel);
            var res = greeterClient.SayHello(new HelloRequest()
            {
                Name = "李斯"
            }); ;
            return Ok(new { success = true, data = res });
        }

    }
}