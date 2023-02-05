using System.Collections.Generic;
using OneBot.Core.Model.Message.SimpleMessageSegment;

namespace OneBot.Core.Model.Message;

public class SimpleMessage : List<IMessageSegment>, Message
{
    public SimpleMessage()
    {

    }

    public SimpleMessage(List<IMessageSegment> t) : base(t)
    {

    }

    public SimpleMessage(IMessageSegment t) : base(new List<IMessageSegment>() { t })
    {

    }

    public SimpleMessage(string str) : base(new List<IMessageSegment>() { SimpleTextSegment.Build(str) })
    {

    }

    public SimpleMessage(char str) : base(new List<IMessageSegment>() { SimpleTextSegment.Build(str) })
    {

    }

    public void Add(string str)
    {
        Add(SimpleTextSegment.Build(str));
    }

    public void Add(char str)
    {
        Add(SimpleTextSegment.Build(str));
    }
}
