using System.Threading.Tasks;
using JetBrains.Annotations;

namespace OneBot.Core.Interface;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IOneBotService
{
    public ValueTask Start();

    public ValueTask Stop();
}
