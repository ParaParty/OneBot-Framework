using System;
using OneBot.Core.Model.Message;
using OneBot.Core.Util;
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
        var ret = new SimpleMessage();

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
        return t.MessageType switch
        {
            SegmentType.Unknown => throw new IgnoreException(),
            SegmentType.Ignore => throw new IgnoreException(),
            SegmentType.Text => SoraTextSegment.Build((TextSegment)t.Data),
            SegmentType.Face => SoraFaceSegment.Build((FaceSegment)t.Data),
            SegmentType.Image => SoraImageSegment.Build((ImageSegment)t.Data),
            SegmentType.Record => SoraRecordSegment.Build((RecordSegment)t.Data),
            SegmentType.Video => SoraVideoSegment.Build((VideoSegment)t.Data),
            SegmentType.Music => SoraMusicSegment.Build((MusicSegment)t.Data),
            SegmentType.At => ((AtSegment)t.Data).Target == "all" ? SoraMentionAllSegment.Build((AtSegment)t.Data) : SoraMentionSegment.Build((AtSegment)t.Data),
            SegmentType.Share => SoraShareSegment.Build((ShareSegment)t.Data),
            SegmentType.Reply => SoraReplySegment.Build((ReplySegment)t.Data),
            SegmentType.Forward => SoraForwardSegment.Build((ForwardSegment)t.Data),
            SegmentType.Poke => SoraPokeSegment.Build((PokeSegment)t.Data),
            SegmentType.Xml => SoraXmlSegment.Build((CodeSegment)t.Data),
            SegmentType.Json => SoraJsonSegment.Build((CodeSegment)t.Data),
            SegmentType.RedBag => SoraRedBagSegment.Build((RedbagSegment)t.Data),
            SegmentType.CardImage => SoraCardImageSegment.Build((CardImageSegment)t.Data),
            SegmentType.TTS => SoraTtsSegment.Build((TtsSegment)t.Data),
            SegmentType.RPS => SoraRpsSegment.Build(t.Data),
            _ => throw new ArgumentException()
        };
    }

    internal static MessageBody ConvertToSoraMessage(this Message t)
    {
        var ret = new MessageBody();

        foreach (var i in t)
        {
            ret.Add(i.ConvertToSoraMessageSegment());
        }

        return ret;
    }

    internal static SoraSegment ConvertToSoraMessageSegment(this MessageSegmentRef t)
    {
        var type = t.GetSegmentType();
        if (type == null)
        {
            throw new ArgumentException();
        }
        return type switch
        {
            "text" => SoraSegment.Text(t.GetText()),
            "face" => SoraSegment.Face(int.Parse(t.Get<string>("face")!)),
            _ => throw new ArgumentException()
        };
    }
}
