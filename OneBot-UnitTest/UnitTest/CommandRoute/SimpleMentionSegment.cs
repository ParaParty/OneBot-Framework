using System;
using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;

namespace OneBot.UnitTest.CommandRoute;

public class SimpleMentionSegment : Mention, UnderlayModel<string>
{
    public SimpleMentionSegment(string userId)
    {
        WrappedModel = userId;
    }

    public string UserId => WrappedModel;

    public string WrappedModel { get; }

    public static MessageSegmentRef Build(char tData)
    {
        return new MessageSegment<Mention>(new SimpleMentionSegment("" + tData));
    }

    public static MessageSegmentRef Build(string tData)
    {
        return new MessageSegment<Mention>(new SimpleMentionSegment(tData));
    }

    public static MessageSegmentRef Build()
    {
        return new MessageSegment<Mention>(new SimpleMentionSegment(String.Empty));
    }
}