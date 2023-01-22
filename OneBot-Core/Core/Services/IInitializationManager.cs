using JetBrains.Annotations;
using OneBot.Core.Event;

namespace OneBot.Core.Services;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IInitializationManager
{
    void Initialize();
}
