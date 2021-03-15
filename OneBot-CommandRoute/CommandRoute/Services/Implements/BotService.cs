using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneBot.CommandRoute.Models.VO;
using Sora.Interfaces;
using Sora.Net;
using Sora.OnebotModel;
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
        public ISoraService SoraService { get; set; }

        /// <summary>
        /// Sora WS 服务器设置
        /// </summary>
        private ISoraConfig ServiceConfig { get; set; }

        /// <summary>
        /// 依赖注入服务
        /// </summary>
        private IServiceProvider ServiceProvider { get; set; }

        public BotService(IOptions<CQHttpServerConfigModel> cqHttpServerConfigModel, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            // 配置日志
            var logger = ServiceProvider.GetService<ILogService>();
            if (logger != null) Log.SetLoggerService(logger);

            // 配置 CQHTTP Sora
            var cqHttpConfig = cqHttpServerConfigModel?.Value;
            ServiceConfig = cqHttpConfig == null ? new ServerConfig() : cqHttpConfig.ToServiceConfig();
            SoraService = SoraServiceFactory.CreateInstance(ServiceConfig);
        }

        public void Start()
        {
            // 初始化指令系统
            var commandService = ServiceProvider.GetService<ICommandService>();
            // ReSharper disable once PossibleNullReferenceException
            commandService.RegisterCommand();

            // 启动 CQHTTP
            SoraService.StartService();
        }
    }
}