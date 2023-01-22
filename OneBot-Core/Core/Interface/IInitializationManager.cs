using JetBrains.Annotations;

namespace OneBot.Core.Interface;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IInitializationManager
{
    void Initialize();
}
