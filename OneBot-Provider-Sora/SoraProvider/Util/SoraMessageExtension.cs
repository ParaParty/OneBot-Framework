using System;
using OneBot.Core.Model.Message;
using OneBot.Provider.SoraProvider.Exceptions;
using OneBot.Provider.SoraProvider.Model.MessageSegment;
using Sora.Entities;
using Sora.Entities.Segment;
using Sora.Entities.Segment.DataModel;
using Sora.Enumeration;

namespace OneBot.Provider.SoraProvider.Util;

public static class SoraMessageExtension
{
    internal static Message ConvertToOneBotMessage(this MessageContext t)
    {
        return t.MessageBody.ConvertToOneBotMessage();
    }

    internal static Message ConvertToOneBotMessage(this MessageBody t)
    {
        var ret = new DefaultMessage();

        foreach (SoraSegment o in t)
        {
            try
            {
                ret.Add(o.ConvertToOneBotMessageSegment());
            }
            catch (IgnoreException)
            {

            }
        }

        return ret;
    }

    internal static MessageSegmentRef ConvertToOneBotMessageSegment(this SoraSegment t)
    {
        switch (t.MessageType)
        {
            case SegmentType.Unknown:
                throw new IgnoreException();
            case SegmentType.Ignore:
                throw new IgnoreException();
            case SegmentType.Text:
                return SoraTextSegment.Build((TextSegment)t.Data);
            case SegmentType.Face:
                break; // TODO
            case SegmentType.Image:
                return SoraImageSegment.Build((ImageSegment)t.Data);
            case SegmentType.Record:
                return SoraRecordSegment.Build((RecordSegment)t.Data);
            case SegmentType.Video:
                return SoraVideoSegment.Build((VideoSegment)t.Data);
            case SegmentType.Music:
                break; // TODO
            case SegmentType.At:
                return ((AtSegment)t.Data).Target == "all" ? SoraMentionAllSegment.Build((AtSegment)t.Data) : SoraMentionSegment.Build((AtSegment)t.Data);
            case SegmentType.Share:
                break; // TODO
            case SegmentType.Reply:
                return SoraReplySegment.Build((ReplySegment)t.Data);
            case SegmentType.Forward:
                break; // TODO
            case SegmentType.Poke:
                break; // TODO
            case SegmentType.Xml:
                break; // TODO
            case SegmentType.Json:
                break; // TODO
            case SegmentType.RedBag:
                break; // TODO
            case SegmentType.CardImage:
                break; // TODO
            case SegmentType.TTS:
                break; // TODO
            case SegmentType.RPS:
                break; // TODO
            default:
                throw new ArgumentException();
        }
        throw new ArgumentException("?");
    }
}
