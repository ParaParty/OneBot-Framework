using System.Reflection;
using OneBot.Core.Attributes;

namespace OneBot.Core.Services;

/// <summary>
/// CQJson 路由服务
/// </summary>
public interface ICQJsonRouterService
{
    /// <summary>
    /// 注册路由
    /// </summary>
    /// <param name="oneBotController"></param>
    /// <param name="method"></param>
    /// <param name="attr"></param>
    void Register(IOneBotController oneBotController, MethodInfo method, CQJsonAttribute attr);

    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="eventArgs"></param>
    /// <param name="appid"></param>
    /// <returns></returns>
    int Handle(OneBotContext scope, BaseSoraEventArgs eventArgs, string appid);
}