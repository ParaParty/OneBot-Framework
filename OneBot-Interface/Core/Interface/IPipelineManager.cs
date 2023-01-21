using System.Threading.Tasks;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

public interface IPipelineManager
{

    ValueTask Handle(OneBotContext ctx);
}
