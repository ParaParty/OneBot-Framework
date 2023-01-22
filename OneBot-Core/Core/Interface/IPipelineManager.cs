using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IPipelineManager
{
    ValueTask Handle(OneBotContext ctx);
}
