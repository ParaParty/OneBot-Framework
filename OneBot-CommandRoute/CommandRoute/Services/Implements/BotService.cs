using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneBot.CommandRoute.Models.VO;
using Sora.Server;

namespace OneBot.CommandRoute.Services.Implements
{
    public class BotService: IBotService
    {
        public SoraWSServer Server { get; set; }

        private ServerConfig ServerConfig { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        public BotService(IOptions<CQHttpServerConfigModel> cqHttpServerConfigModel, IServiceProvider serviceProvider)
        {
            // 配置 CQHTTP
            var cqHttpConfig = cqHttpServerConfigModel?.Value;
            ServerConfig = cqHttpConfig == null ? new ServerConfig() : cqHttpConfig.ToServerConfig();
            Server = new SoraWSServer(ServerConfig);

            // 配置指令
            ServiceProvider = serviceProvider;
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