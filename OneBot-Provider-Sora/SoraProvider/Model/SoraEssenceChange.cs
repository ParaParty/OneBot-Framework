using System;
using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraEssenceChange : EssenceChange, UnderlaySoraEvent<EssenceChangeEventArgs>
{
    public SoraEssenceChange(EssenceChangeEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;

        SubType = WrappedModel.EssenceChangeType switch
        {

            EssenceChangeType.Add => EssenceChange.SubType.Add,
            EssenceChangeType.Delete => EssenceChange.SubType.Delete,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string SubType { get; init; }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string MessageId => WrappedModel.MessageId.ToString();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string OperatorId => WrappedModel.Operator.Id.ToString();

    public EssenceChangeEventArgs WrappedModel { get; }
}
