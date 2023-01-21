using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneBot.Core.Configuration;
using OneBot.Core.Mixin;
using OneBot.FrameworkDemo.Middleware;

namespace OneBot.FrameworkDemo;

public class Program
{
    public static void Main(string[] args)
    {
        // 创建主机
        var builder = Host.CreateDefaultBuilder(args);

        // 配置 OneBot 主机
        builder.ConfigureOneBotHost();

        // 配置服务
        builder.ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;

            services.AddOneBot(ob =>
            {
                ob.ConfigurePipeline(pipe =>
                {
                    services.AddSingleton<TestMiddleware>();
                    pipe.Use(typeof(TestMiddleware));
                });
            });
        });

        // 开始运行
        builder.Build().Run();
    }
}
