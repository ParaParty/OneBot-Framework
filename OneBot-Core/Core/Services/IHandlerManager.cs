﻿using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Services;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IHandlerManager
{
    ValueTask Handle(OneBotContext e);
}
