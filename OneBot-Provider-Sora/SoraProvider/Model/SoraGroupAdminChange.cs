using System;
using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupAdminChange : GroupAdminChange, UnderlaySoraEvent<GroupAdminChangeEventArgs>
{
    public SoraGroupAdminChange(GroupAdminChangeEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;

        SubType = WrappedModel.SubType switch
        {
            AdminChangeType.Set => GroupAdminChange.SubType.Set,
            AdminChangeType.UnSet => GroupAdminChange.SubType.Unset,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string SubType { get; init; }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public GroupAdminChangeEventArgs WrappedModel { get; }
}