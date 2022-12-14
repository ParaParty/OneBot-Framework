using System;
using Microsoft.Extensions.DependencyInjection;
using Sora;
using Sora.Interfaces;
using Sora.Net.Config;
using YukariToolBox.LightLog;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

/// <summary>
/// CQHTTP 客户端（Sora）
/// </summary>
public class SoraProviderService : ISoraProviderService
{
    /// <summary>
    /// Sora WS 服务
    /// </summary>
    public ISoraService SoraService { get; }

    /// <summary>
    /// Sora WS 服务设置
    /// </summary>
    public ISoraConfig ServiceConfig { get; }

    public SoraProviderService(IServiceProvider serviceProvider)
    {
        // 配置日志
        var logger = serviceProvider.GetService<ILogService>();
        Log.LogConfiguration.DisableConsoleOutput();
        if (logger != null) Log.LogConfiguration.AddLogService(logger);

        // 配置 CQHTTP Sora
        var soraConfig = serviceProvider.GetService<ISoraConfig>();
        ServiceConfig = soraConfig ?? new ServerConfig();
        SoraService = SoraServiceFactory.CreateService(ServiceConfig);
    }
}
