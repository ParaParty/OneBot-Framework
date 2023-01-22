using System.Threading.Tasks;
using OneBot.Core.Context;

namespace OneBot.Core.Services;

public interface IPipelineManager
{
    ValueTask Handle(OneBotContext ctx);
}
