using Microsoft.Extensions.DependencyInjection;
using OneBot.CommandRoute.Services;
using OneBot.CommandRoute.Services.Implements;
using OneBot_CommandRoute.CommandRoute.Utils;
using YukariToolBox.FormatLog;

namespace OneBot_CommandRoute.CommandRoute.Mixin
{
    public static class StartupMixin
    {
        /// <summary>
        /// 将 OneBot 服务注册到服务容器。包含：注册 OneBot 服务、指令服务、CQ:Json 服务和日志服务。
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureOneBot(this IServiceCollection services)
        {            
            services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<ICQJsonRouterService, CQJsonRouterService>();
            services.AddSingleton<ILogService, YukariToolBoxLogger>();
        }
    }
}