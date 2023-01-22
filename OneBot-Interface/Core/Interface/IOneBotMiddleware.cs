using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

/// <summary>
/// OneBot 中间件
/// </summary>
[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IOneBotMiddleware
{
    /// <summary>
    /// OneBot 中间件处理
    /// </summary>
    /// <param name="ctx">OneBot 事件上下文</param>
    /// <param name="next">下一步</param>
    /// <returns></returns>
    public ValueTask Invoke(OneBotContext ctx, OneBotEventDelegate next);
}
