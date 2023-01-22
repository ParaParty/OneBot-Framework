using JetBrains.Annotations;
using OneBot.Core.Event;

namespace OneBot.Core.Interface;

/// <summary>
/// 被标记为 <code>INeedInitialization</code> 的全局单例将会在启动 PlatformProvider 之前被实例化并调用 Initialize 方法。
/// </summary>
[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface INeedInitialization
{
    void Initialize() { }
}
