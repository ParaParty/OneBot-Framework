using OneBot.Core.Interface;

namespace OneBot.Core.Util;

public static class MiddlewareUtil
{
    public static string ToDiagnosisName(this IOneBotMiddleware t)
    {
        return t.GetType().FullName ?? "?";
    }
}
