using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

        public bool IsEmpty()
        {
            return Segment <= 0 && Position <= 0;
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
        //     var str = lastElement.GetText();
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
                var str = seg.GetText() ?? throw new ArgumentException();
                ret.Add(SimpleTextSegment.Build(str.Substring(start.Position, end.Position - start.Position)));
            }

            throw new ArgumentException();
        }

        if (start.Segment > end.Segment)
        {
            throw new ArgumentException();
        }

        if (start.Position > 0)
        {
            var startSeg = this[start.Segment];
            if (startSeg.GetSegmentType() != "text")
            {
                throw new ArgumentException();
            }
            var str = startSeg.GetText() ?? throw new ArgumentException();
            ret.Add(SimpleTextSegment.Build(str.Substring(start.Position)));
        }

        for (int i = start.Position > 0 ? start.Segment + 1 : start.Segment; i < end.Segment; i++)
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
            var str = endSeg.GetText() ?? throw new ArgumentException();
            ret.Add(SimpleTextSegment.Build(str.Substring(0, end.Position)));
        }

        return ret;
    }

    public class Builder
    {
        private readonly SimpleMessage _buffer = new SimpleMessage();

        private StringBuilder _sb = new StringBuilder();

        public Builder Add(string s)
        {
            _sb.Append(s);
            return this;
        }

        public Builder Add(char s)
        {
            _sb.Append(s);
            return this;
        }

        public Builder Add(MessageSegmentRef seg)
        {
            if (seg.GetSegmentType() == "text")
            {
                var str = seg.GetText() ?? throw new ArgumentException();
                _sb.Append(str);
            }
            else
            {
                FlushStringBuilder();
                _buffer.Add(seg);

            }
            return this;
        }

        public Message ToMessage()
        {
            FlushStringBuilder();
            return _buffer;
        }

        private void FlushStringBuilder()
        {
            if (_sb.Length > 0)
            {
                _buffer.Add(_sb.ToString());
            }
            _sb = new StringBuilder();
        }
    }

    static readonly Message EmptyMessage = new EmptyMessageType();

    public class EmptyMessageType : Message
    {
        private class EmptyMessageEnumerator :IEnumerator<MessageSegmentRef>
        {
            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
                
            }

            public MessageSegmentRef Current => throw new NullReferenceException(); 

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
        
        internal EmptyMessageType()
        {

        }

        public IEnumerator<MessageSegmentRef> GetEnumerator()
        {
            return new EmptyMessageEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => 0;

        public MessageSegmentRef this[int index] => throw new IndexOutOfRangeException();
    }
}
