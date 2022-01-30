using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneBot.CommandRoute.Mixin;
using OneBot.CommandRoute.Models.VO;
using OneBot.CommandRoute.Services;
using OneBot.FrameworkDemo.Middleware;
using OneBot.FrameworkDemo.Modules;

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
            // 配置机器人核心
            // 设置 OneBot 配置
            services.Configure<CQHttpServerConfigModel>(configuration.GetSection("CQHttpConfig"));
            services.ConfigureOneBot();

            // 添加中间件
            // 单例模式或原型模式都可以，问题不大。
            services.AddScoped<IOneBotMiddleware, TestMiddleware>();

            // 添加指令 / 事件
            // 推荐使用单例模式（而实际上框架代码也是当单例模式使用的）
            services.AddSingleton<IOneBotController, TestModule>();
            // 一行一行地将指令模块加进去
        });

        // 开始运行
        builder.Build().Run();
    }
}
