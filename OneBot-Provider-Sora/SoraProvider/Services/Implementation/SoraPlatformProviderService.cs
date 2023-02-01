using System;
using System.Threading.Tasks;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Model;
using OneBot.Provider.SoraProvider.Configuration;
using Sora;
using Sora.Interfaces;
using YukariILogService = YukariToolBox.LightLog.ILogService;
using YukariLog = YukariToolBox.LightLog.Log;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

/// <summary>
/// CQHTTP 客户端（Sora）
/// </summary>
public class SoraPlatformProviderService : ISoraPlatformProviderService
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
    /// Sora 事件处理
    /// </summary>
    public SoraHandler SoraHandler { get; }

    /// <summary>
    /// Sora 连接管理器
    /// </summary>
    public SoraConnectionManager ConnectionManager { get; }

    public SoraActionManager ActionManager { get; }

    public SoraPlatformProviderService(SoraConfiguration cfg, IEventDispatcher dispatcher)
    {
        YukariLog.LogConfiguration.DisableConsoleOutput();
        if (cfg.Logger != null)
        {
            YukariLog.LogConfiguration.AddLogService(cfg.Logger);
        }

        ServiceConfig = cfg.SoraConfig;
        SoraService = SoraServiceFactory.CreateService(ServiceConfig);

        ConnectionManager = new SoraConnectionManager();
        SoraHandler = new SoraHandler(SoraService, dispatcher, cfg, ConnectionManager);
        ActionManager = new SoraActionManager();
    }

    public ValueTask Start()
    {
        return SoraService.StartService();
    }

    public ValueTask Stop()
    {
        return SoraService.StopService();
    }

    public async ValueTask<OneBotActionResponse> DoAction(IServiceProvider sp, string name, OneBotActionRequest request)
    {
        var nameStep = name.Split(":", 2);
        if (nameStep.Length != 2)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        var soraApi = ConnectionManager.GetClient(nameStep[1]);
        if (soraApi == null)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        return await ActionManager.DoAction(sp, soraApi, request);
    }
}
