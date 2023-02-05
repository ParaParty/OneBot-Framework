using System;
using OneBot.Core.Model.Message;

namespace OneBot.Core.Util;

public static class MessageSegmentExtension
{
    public static bool TypeIsText(this IMessageSegment it)
    {
        return it.GetSegmentType() == "text";
    }

    public static string? GetString(this IMessageSegment it, string key)
    {
        var data = it.Get(key);
        return data?.ToString();
    }
    
    public static int GetInt(this IMessageSegment it, string key)
    {
        var data = it.Get(key);
        return Convert.ToInt32(data);
    }
    
    public static long GetLong(this IMessageSegment it, string key)
    {
        var data = it.Get(key);
        return Convert.ToInt64(data);
    }

    public static string? GetText(this IMessageSegment it)
    {
        return it.Get<string>("text");
    }

    public static string? GetFileId(this IMessageSegment it)
    {
        return it.Get<string>("file_id");
    }

    public static string? GetUserId(this IMessageSegment it)
    {
        return it.Get<string>("user_id");
    }
}
