using System.Threading.Tasks;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

/// <summary>
/// OneBot 拦截器传递对象，在 OneBot 中间件中直接调用表示将该时间传递给下一层处理。
/// </summary>
public delegate ValueTask OneBotEventDelegate(OneBotContext context);