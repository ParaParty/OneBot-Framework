using JetBrains.Annotations;

namespace OneBot.Core.Const;

public static class Flags
{
    public const ImplicitUseTargetFlags AllImplicitUseTargetFlags = ImplicitUseTargetFlags.Itself | ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors;
}
