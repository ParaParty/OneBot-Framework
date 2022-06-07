using System.Threading.Tasks;
using Sora.Interfaces;

namespace OneBot.CommandRoute.Services;

/// <summary>
/// OneBot 客户端（Sora）
/// </summary>
public interface IBotService
{
    /// <summary>
    /// Sora WS 服务
    /// </summary>
    ISoraService SoraService { get; }

    /// <summary>
    /// Sora WS 服务设置
    /// </summary>
    public ISoraConfig ServiceConfig { get; }
}