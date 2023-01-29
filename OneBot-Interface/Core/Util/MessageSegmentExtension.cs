using OneBot.Core.Model.Message;

namespace OneBot.Core.Util;

public static class MessageSegmentExtension
{
    public static bool TypeIsText(this MessageSegmentRef it)
    {
        return it.GetSegmentType() == "text";
    }

    public static string? GetText(this MessageSegmentRef it)
    {
        return it.Get<string>("Text");
    }
}
