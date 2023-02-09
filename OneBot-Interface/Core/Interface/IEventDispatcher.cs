using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Event;

namespace OneBot.Core.Interface;

/// <summary>
/// 事件分发器，平台适配器通过本接口向事件处理流程发起一个新的事件
/// </summary>
[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IEventDispatcher
{
    ValueTask Dispatch(string platformName, OneBotEvent e);
}
