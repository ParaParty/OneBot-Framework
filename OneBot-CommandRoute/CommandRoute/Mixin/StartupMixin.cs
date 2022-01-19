using Microsoft.Extensions.DependencyInjection;

using OneBot.CommandRoute.OneBotControllers;
using OneBot.CommandRoute.Services;
using OneBot.CommandRoute.Services.Implements;
using OneBot.CommandRoute.Utils;

using YukariToolBox.LightLog;

namespace OneBot.CommandRoute.Mixin
{
    public static class StartupMixin
    {
        /// <summary>
        /// 将 OneBot 服务注册到服务容器。包含：OneBot 服务、指令服务、CQ:Json 服务和日志服务。
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureOneBot(this IServiceCollection services)
        {
            services
            // OneBot
                .AddSingleton<IBotService, BotService>()

            // 事件服务
                .AddSingleton<IEventService, EventService>()

            // 指令路由服务
                .AddSingleton<ICommandService, CommandService>()

            // CQ:Json 路由服务
                .AddSingleton<ICQJsonRouterService, CQJsonRouterService>()
                .AddSingleton<IOneBotController, CQJsonRouterController>()

            // 日志服务
                .AddSingleton<ILogService, YukariToolBoxLogger>();
        }
    }
}
