using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blade.Grpc;
using grpc.user;
using Blade.Grpc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.process.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    { 
        private IBladeGrpcFactory _bladeGrpcFactory;
        public DefaultController(IBladeGrpcFactory bladeGrpcFactory)
        {

            _bladeGrpcFactory = bladeGrpcFactory; 

        }
        //private Greeter.GreeterClient greeterClient;
        //private User.UserClient userClient;

        //public DefaultController(Greeter.GreeterClient greeter,
        //    User.UserClient user)
        //{
        //    greeterClient = greeter;
        //    userClient = user;  
        //}
        static ILoggerFactory loggerFactory = LoggerFactory.Create(logging =>
        {
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });
        static GrpcChannel channel = GrpcChannel.ForAddress("http://192.168.0.121:7001/", new GrpcChannelOptions() { LoggerFactory = loggerFactory });


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("say")]
        public async Task<IActionResult> Say()
        {
            //var httpClientHandler = new HttpClientHandler();
            //// Return `true` to allow certificates that are untrusted/invalid
            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //var httpClient = new HttpClient(httpClientHandler);

            //var channel = GrpcChannel.ForAddress("http://192.168.0.121:7001/", new GrpcChannelOptions { HttpClient = httpClient });



            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            //stopwatch.Start();
             

            var greeterClient = new Greeter.GreeterClient(channel);
            var userClient = new User.UserClient(channel);
            //for (int i = 0; i < 1000; i++)
            //{

            //var res = await greeterClient.SayHelloAsync(new HelloRequest()
            //    {
            //        Name = "李斯"
            //    }); ;
            //    var res2 = await userClient.GetUserAsync(new UserRequest()
            //    {
            //        Id = "1"
            //    });
            //}
            var res = await greeterClient.SayHelloAsync(new HelloRequest()
            {
                Name = "李斯"
            }); ;
            var res2 = await userClient.GetUserAsync(new UserRequest()
            {
                Id = "1"
            });
            //stopwatch.Stop();

            //return Ok();
            return Ok(new { success = true, data = res, dta = res2 });
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var channel = _bladeGrpcFactory.Create<User.UserClient>().Result;
            var userClient = new User.UserClient(channel);
            var res2 = await userClient.GetUserAsync(new UserRequest()
            {
                Id = "1"
            });
            System.Diagnostics.Debug.WriteLine("controller action结束");
            return Ok(new { dta = res2 });
        }


    }
}