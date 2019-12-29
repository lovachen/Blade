# Blade.Grpc
一个实用于 asp.net core 微服务业务逻辑层的Grpc管道创建者，结合Consul服务发现动态构建GrpcChannel管道。
  - 3.0 后 asp.net core 引入了 高性能 Grpc ，注意：需要使用grpc的异步方法调用获得足够的性能

## 特点
  - Consul 服务发现
  - 负载均衡
 
## 安装使用
Blade.Grpc 适用 asp.net core 3.1 或及以后的版本。  
使用 NuGet 安装 Blade.Grpc   

 `Install-Package Blade.Grpc`  
 
 `Install-Package Blade.Grpc.Provider.Consul`
 
 ## 快速使用
 
 1. 在 Startup.ConfigureServices 中添加服务
 
    `services.AddBlade(Configuration).AddConsul();`
 
 2. 在 Startup.Configure 中启用  
 
    `app.UseBlade();`
 
 3. DI 注入使用方式 通过 IBladeGrpcFactory 接口即可构建GrpcChannel  
 
    ```
    private IBladeGrpcFactory _bladeGrpcFactory; 
    
    public DefaultController(IBladeGrpcFactory bladeGrpcFactory)  
    {  
        _bladeGrpcFactory = bladeGrpcFactory;   
    } 
      
    ```
    
    ```
    1. 通过 Consul 服务名称创建 GprcChanne 管道
      GrpcChannel channel =  _bladeGrpcFactory.Create(Consul 注册的 服务名称);
        
    ```
    
    ```
    2. 为了便以方便 创建管道的方式还可以通过 Create<T> 的方式 来进行服务对比构建 使用此方法时 先要配置 GrpcProfile 
    
       1. 构建一个类 MapGrpcProfile : GrpcProfile
       
       public class MapGrpcProfile: GrpcProfile
        {
            public MapGrpcProfile()
            { 
                Add<Greeter.GreeterClient>("consul.service.name"); 
            }
        }
        
       2. 服务配置时 使用 如下
       services.AddBlade(Configuration).AddConsul().AddGrpcProfile<MapGrpcProfile>();
      
      即可通过如下方式构建GrpcChannel管道
      GrpcChannel channel =  _bladeGrpcFactory.Create<Greeter.GreeterClient>();
    ```

 4.  添加 appsettings.json 配置
    
        ```
          "BladeGrpc": {
          "ServiceDiscoveryProvider": {
            "Host": "consul地址",
            "Port": 8500,
            "Listening": true,  //长轮询方式,
            "PollingInterval":100, //论时间 ms ，默认100ms
            "LoadBalancerOptions": {
              "Type": "LeastConnection"
            }
          },
          "Downstream": [
            {
              "ServiceName": "consul 注册的服务名称"
            }
          ]
        }

       ```

