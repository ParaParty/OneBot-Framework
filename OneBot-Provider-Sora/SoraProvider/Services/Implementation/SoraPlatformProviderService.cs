using System.Threading.Tasks;
using OneBot.Core.Interface;
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

    public SoraPlatformProviderService(SoraConfiguration cfg, IEventDispatcher dispatcher)
    {
        YukariLog.LogConfiguration.DisableConsoleOutput();
        if (cfg.Logger != null)
        {
            YukariLog.LogConfiguration.AddLogService(cfg.Logger);
        }

        ServiceConfig = cfg.SoraConfig;
        SoraService = SoraServiceFactory.CreateService(ServiceConfig);

        SoraHandler = new SoraHandler(SoraService, dispatcher);
    }

    public ValueTask Start()
    {
        return SoraService.StartService();
    }

    public ValueTask Stop()
    {
        return SoraService.StopService();
    }
}
