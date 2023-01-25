using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneBot.CommandRoute.Configuration;
using OneBot.Core.Configuration;
using OneBot.Core.Context;
using OneBot.Core.Mixin;
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

            services.AddOneBot(ob =>
            {
                ob.AddOneBotCore(s =>
                {
                    s.AddArgumentDependencyInjectionResolver();
                });

                ob.AddCommandRoute(s =>
                {
                    s.Route(route =>
                    {
                        route.Middleware(typeof(TestMiddleware)); 
                        
                        route.Group("shop", shopRoute =>
                        {
                            route.Middleware(typeof(TestMiddleware2)); 
                            shopRoute.Command("page [page]", typeof(ShopController), "ListGood");
                            // shopRoute.Command("help", typeof(ShopController), "Help");
                            shopRoute.Command<ShopController>("help", c => c.Help);
                            shopRoute.Command("buy <name>", (OneBotContext ctx) =>
                            {
                                // 可以选择直接在路由里写指令逻辑
                            });

                            route.Group("good add <name> <price>", r =>
                            {
                                route.Middleware(typeof(TestNeedAdminMiddleware));
                                shopRoute.Fallback(typeof(ShopController), "AddGood");
                            });
                            
                            shopRoute.Fallback(typeof(ShopController), "CommandNotFound");
                        });
                    });

                    s.AddLagencyController(); // 当然我们也要兼容以前的用法，点击开始扫包。
                });
                
                ob.ConfigurePipeline(pipe =>
                {
                    services.AddSingleton<TestMiddleware>();
                    pipe.Use<TestMiddleware>();
                    pipe.UseCommandRoute();
                    pipe.UseEventHandler();
                });
            });
        });

        // 开始运行
        builder.Build().Run();
    }
}
