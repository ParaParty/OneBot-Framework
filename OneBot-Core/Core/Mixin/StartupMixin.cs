using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OneBot.Core.Mixin;

public static class StartupMixin
{
    /// <summary>
    /// 将 OneBot 服务注册到服务容器。
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureOneBot(this IServiceCollection services)
    {
        // services.AddSingleton<IHostedService, OneBotHostedService>();
    }


    public static IHostBuilder ConfigureOneBotHost(this IHostBuilder builder)
    {
        // builder.ConfigureServices(s => s.AddSingleton<IServer, OneBotServer>());
        return builder;
    }
}