using System;
using System.Collections.Generic;
using OneBot.Core.Model.Message.SimpleMessageSegment;
using OneBot.Core.Util;

namespace OneBot.Core.Model.Message;

public interface Message : IReadOnlyList<MessageSegmentRef>
{
    public struct Index
    {
        public int Segment { get; }

        public int Position { get; }

        public Index(int segment, int position)
        {
            Segment = segment;
            Position = position;
        }
    }

    public Index MessageLength()
    {
        var seg = Count;
        if (seg == 0)
        {
            return new Index(0, 0);
        }

        // var lastElement = this[seg - 1];
        // if (lastElement.GetSegmentType() == "text")
        // {
        //     var str = lastElement.Get<string>("Message");
        //     return new Index(seg - 1, str?.Length ?? 0);
        // }
        return new Index(seg, 0);
    }

    public Message SubMessage(Index start)
    {
        return SubMessage(start, MessageLength());

    }
    public Message SubMessage(Index start, Index end)
    {
        var ret = new SimpleMessage();

        if (start.Segment == end.Segment)
        {
            if (start.Position == end.Position)
            {
                ret.Add(SimpleTextSegment.Build());
                return ret;
            }

            if (start.Position < end.Position)
            {
                var seg = this[start.Segment];
                if (seg.GetSegmentType() != "text")
                {
                    throw new ArgumentException();
                }
                var str = seg.Get<string>("Message") ?? throw new ArgumentException();

                ret.Add(SimpleTextSegment.Build(str.Substring(start.Position, end.Position)));
            }

            throw new ArgumentException();
        }

        if (start.Position > 0)
        {
            var startSeg = this[start.Segment];
            if (startSeg.GetSegmentType() != "text")
            {
                throw new ArgumentException();
            }
            var str = startSeg.Get<string>("Message") ?? throw new ArgumentException();
            ret.Add(SimpleTextSegment.Build(str.Substring(start.Position)));
        }

        for (int i = start.Segment + 1; i < end.Segment; i++)
        {
            ret.Add(this[i]);
        }

        if (end.Position > 0)
        {
            var endSeg = this[end.Segment];
            if (endSeg.GetSegmentType() != "text")
            {
                throw new ArgumentException();
            }
            var str = endSeg.Get<string>("Message") ?? throw new ArgumentException();
            ret.Add(SimpleTextSegment.Build(str.Substring(0, end.Position)));
        }
        
        return ret;
    }
}
