using OneBot.Core.Model;

namespace OneBot.Core.Util;

public static class IOneBotActionExtension
{
    public static T? Get<T>(this OneBotActionRequest self, string key)
    {
        var t = self.Params;
        if (t == null)
        {
            return default;
        }
        return (T?)PropertyAccessor.Get(t, key);
    }

    public static T? Get<T>(this IOneBotActionRequestParams self, string key)
    {
        return (T?)PropertyAccessor.Get(self, key);
    }
    public static T? Get<T>(this OneBotActionResponse self, string key)
    {
        var t = self.Data;
        if (t == null)
        {
            return default;
        }
        return (T?)PropertyAccessor.Get(t, key);
    }

    public static T? Get<T>(this IOneBotActionResponseData self, string key)
    {
        return (T?)PropertyAccessor.Get(self, key);
    }
}
