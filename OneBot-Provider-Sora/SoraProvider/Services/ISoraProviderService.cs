using Sora.Interfaces;

namespace OneBot.Provider.SoraProvider.Services;

/// <summary>
/// OneBot 客户端（Sora）
/// </summary>
public interface ISoraProviderService
{
    /// <summary>
    /// Sora WS 服务
    /// </summary>
    Sora.Interfaces.ISoraService SoraService { get; }

    /// <summary>
    /// Sora WS 服务设置
    /// </summary>
    public ISoraConfig ServiceConfig { get; }
}