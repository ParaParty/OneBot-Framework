using System.Collections.Generic;
using OneBot.Core.Model.Message.SimpleMessageSegment;

namespace OneBot.Core.Model.Message;

public class SimpleMessage : List<MessageSegmentRef>, Message
{
    public SimpleMessage()
    {

    }

    public SimpleMessage(List<MessageSegmentRef> t) : base(t)
    {

    }

    public SimpleMessage(MessageSegmentRef t) : base(new List<MessageSegmentRef>() { t })
    {

    }

    public SimpleMessage(string str) : base(new List<MessageSegmentRef>() { SimpleTextSegment.Build(str) })
    {

    }
    public SimpleMessage(char str) : base(new List<MessageSegmentRef>() { SimpleTextSegment.Build(str) })
    {

    }
}
