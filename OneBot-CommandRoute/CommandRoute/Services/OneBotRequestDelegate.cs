using System.Threading.Tasks;
using OneBot.CommandRoute.Models;

namespace OneBot.CommandRoute.Services;

/// <summary>
/// OneBot 拦截器传递对象，在 OneBot 中间件中直接调用表示将该时间传递给下一层处理。
/// </summary>
public delegate ValueTask OneBotRequestDelegate(OneBotContext context);
