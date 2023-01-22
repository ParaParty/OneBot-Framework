using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Event;
using OneBot.Core.Model;

namespace OneBot.Core.Interface;

/// <summary>
/// 事件分发器，平台适配器通过本类提供的方法向事件处理流程发起一个新的事件
/// </summary>
[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)] 
public interface IEventDispatcher
{
    ValueTask Dispatch(OneBotEvent e);
}
