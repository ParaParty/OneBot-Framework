using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneBot.CommandRoute.Models.VO;
using OneBot_CommandRoute.CommandRoute.Utils;
using Sora.Server;
using YukariToolBox.FormatLog;

namespace OneBot.CommandRoute.Services.Implements
{
    /// <summary>
    /// CQHTTP 客户端（Sora）
    /// </summary>
    public class BotService : IBotService
    {
        /// <summary>
        /// Sora WS 服务器
        /// </summary>
        public SoraWSServer Server { get; set; }

        /// <summary>
        /// Sora WS 服务器设置
        /// </summary>
        private ServerConfig ServerConfig { get; set; }

        /// <summary>
        /// 依赖注入服务
        /// </summary>
        private IServiceProvider ServiceProvider { get; set; }

        public BotService(IOptions<CQHttpServerConfigModel> cqHttpServerConfigModel, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            // 配置日志
            var logger = ServiceProvider.GetService<ILogService>();
            if (logger != null) YukariToolBox.FormatLog.Log.SetLoggerService(logger);

            // 配置 CQHTTP
            var cqHttpConfig = cqHttpServerConfigModel?.Value;
            ServerConfig = cqHttpConfig == null ? new ServerConfig() : cqHttpConfig.ToServerConfig();
            Server = new SoraWSServer(ServerConfig);
        }

        public void Start()
        {
            // 初始化指令系统
            var commandService = ServiceProvider.GetService<ICommandService>();
            commandService.RegisterCommand();

            // 启动 CQHTTP
            Server.StartServer();
        }
    }
}