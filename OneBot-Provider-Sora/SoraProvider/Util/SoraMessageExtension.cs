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
                break;
            case SegmentType.Image:
                break;
            case SegmentType.Record:
                break;
            case SegmentType.Video:
                break;
            case SegmentType.Music:
                break;
            case SegmentType.At:
                break;
            case SegmentType.Share:
                break;
            case SegmentType.Reply:
                break;
            case SegmentType.Forward:
                break;
            case SegmentType.Poke:
                break;
            case SegmentType.Xml:
                break;
            case SegmentType.Json:
                break;
            case SegmentType.RedBag:
                break;
            case SegmentType.CardImage:
                break;
            case SegmentType.TTS:
                break;
            case SegmentType.RPS:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}
