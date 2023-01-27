using System;
using OneBot.Core.Model.Message.MessageSegmentData;

namespace OneBot.Core.Model.Message.SimpleMessageSegment;

public class SimpleTextSegment : Text, UnderlayModel<string>
{
    public SimpleTextSegment(string data)
    {
        WrappedModel = data;
    }

    public string Text => WrappedModel;

    public string WrappedModel { get; }

    public static MessageSegmentRef Build(char tData)
    {
        return new MessageSegment<Text>(new SimpleTextSegment("" + tData));
    }

    public static MessageSegmentRef Build(string tData)
    {
        return new MessageSegment<Text>(new SimpleTextSegment(tData));
    }

    public static MessageSegmentRef Build()
    {
        return new MessageSegment<Text>(new SimpleTextSegment(String.Empty));
    }
}
