using OneBot.Core.Context;
using OneBot.Core.Util;

namespace OneBot.FrameworkDemo.Modules;

public class ShopController
{
    public void Help(OneBotContext ctx)
    {
        ctx.Reply("test");
    }
}
