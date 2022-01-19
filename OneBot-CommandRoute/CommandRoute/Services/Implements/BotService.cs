using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OneBot.CommandRoute.Models.VO;

using Sora;
using Sora.Interfaces;
using Sora.Net.Config;

using YukariToolBox.LightLog;

namespace OneBot.CommandRoute.Services.Implements
{
    /// <summary>
    /// CQHTTP 客户端（Sora）
    /// </summary>
    public class BotService : IBotService
    {
        /// <summary>
        /// Sora WS 服务
        /// </summary>
        public ISoraService SoraService { get; }

        /// <summary>
        /// Sora WS 服务设置
        /// </summary>
        public ISoraConfig ServiceConfig { get; }

        /// <summary>
        /// 依赖注入服务
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        ///// <summary>
        ///// OneBot 启动后的 Task
        ///// </summary>
        //private ValueTask _startTask;

        public BotService(IOptions<CQHttpServerConfigModel> cqHttpServerConfigModel, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // 配置日志
            var logger = _serviceProvider.GetService<ILogService>();
            Log.LogConfiguration.DisableConsoleOutput();
            if (logger != null) Log.LogConfiguration.AddLogService(logger);

            // 配置 CQHTTP Sora
            var cqHttpConfig = cqHttpServerConfigModel.Value;
            // cqHttpConfig is not null here.
            ServiceConfig = cqHttpConfig == null ? new ServerConfig() : cqHttpConfig.ToServiceConfig();
            SoraService = SoraServiceFactory.CreateService(ServiceConfig);
        }

        public void Start()
        {
            // 初始化指令系统
            var commandService = _serviceProvider.GetRequiredService<ICommandService>();
                                 //throw new ArgumentNullException("", "ICommandService did not register.");
            commandService.RegisterCommand();

            // 初始化事件系统
            var eventService = _serviceProvider.GetRequiredService<IEventService>();
                                 //throw new ArgumentNullException("", "IEventService did not register.");
            eventService.RegisterEventHandler();

            //// 启动 CQHTTP
            //_startTask = SoraService.StartService();
        }
    }
}
