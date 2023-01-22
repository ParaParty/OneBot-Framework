using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.UnitTest.Event;

[OneBotTypeProperty("test", "test")]
public class MinimalEvent : OneBotEvent
{
    public string Id => "0";

    public double Time => 0.0;
}
