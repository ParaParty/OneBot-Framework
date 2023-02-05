using System;
using OneBot.Core.Model.Message.MessageSegmentData;

namespace OneBot.Core.Model.Message.SimpleMessageSegment;

public class SimpleTextSegment : Text, UnderlayModel<string>
{
    public SimpleTextSegment(string data) : base(data)
    {
        WrappedModel = data;
    }

    public string Text => WrappedModel;

    public string WrappedModel { get; }

    public static IMessageSegment Build(char tData)
    {
        return new MessageSegment(new SimpleTextSegment("" + tData));
    }

    public static IMessageSegment Build(string tData)
    {
        return new MessageSegment(new SimpleTextSegment(tData));
    }

    public static IMessageSegment Build()
    {
        return new MessageSegment(new SimpleTextSegment(String.Empty));
    }
}
