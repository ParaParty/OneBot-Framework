using System;
using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;

namespace OneBot.UnitTest.CommandRoute;

public class SimpleMentionSegment : Mention, UnderlayModel<string>
{
    public SimpleMentionSegment(string userId):base(userId)
    {
        WrappedModel = userId;
    }

    public string WrappedModel { get; }

    public static IMessageSegment Build(char tData)
    {
        return new MessageSegment(new SimpleMentionSegment("" + tData));
    }

    public static IMessageSegment Build(string tData)
    {
        return new MessageSegment(new SimpleMentionSegment(tData));
    }

    public static IMessageSegment Build()
    {
        return new MessageSegment(new SimpleMentionSegment(String.Empty));
    }
}
