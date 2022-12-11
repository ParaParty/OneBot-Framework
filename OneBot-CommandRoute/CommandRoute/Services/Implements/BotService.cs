using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneBot.CommandRoute.Models.VO;
using Sora;
using Sora.Interfaces;
using Sora.Net.Config;
using YukariToolBox.LightLog;

namespace OneBot.CommandRoute.Services.Implements;

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

    public BotService(IOptions<CQHttpServerConfigModel> cqHttpServerConfigModel, IServiceProvider serviceProvider)
    {
        // 配置日志
        var logger = serviceProvider.GetService<ILogService>();
        Log.LogConfiguration.DisableConsoleOutput();
        if (logger != null) Log.LogConfiguration.AddLogService(logger);

        // 配置 CQHTTP Sora
        var cqHttpConfig = cqHttpServerConfigModel.Value;
        ServiceConfig = cqHttpConfig == null ? new ServerConfig() : cqHttpConfig.ToServiceConfig();
        SoraService = SoraServiceFactory.CreateService(ServiceConfig);
    }
}