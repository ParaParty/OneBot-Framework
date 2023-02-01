using System;
using System.Threading.Tasks;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Model;
using Sora.Interfaces;

namespace OneBot.Provider.SoraProvider.Services;

/// <summary>
/// OneBot 客户端（Sora）
/// </summary>
public interface ISoraPlatformProviderService : IPlatformProvider
{
    /// <summary>
    /// Sora WS 服务
    /// </summary>
    ISoraService SoraService { get; }

    /// <summary>
    /// Sora WS 服务设置
    /// </summary>
    public ISoraConfig ServiceConfig { get; }

    /// <summary>
    /// 执行机器人 Action
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="name"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<OneBotActionResponse> DoAction(IServiceProvider sp, string name, OneBotActionRequest request);
}
