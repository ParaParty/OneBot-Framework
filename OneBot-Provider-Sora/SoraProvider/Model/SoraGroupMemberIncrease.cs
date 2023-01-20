using System;
using OneBot.Core.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupMemberIncrease : GroupMemberIncrease, UnderlaySoraEvent<GroupMemberChangeEventArgs>
{
    public SoraGroupMemberIncrease(GroupMemberChangeEventArgs wrappedModel)
    {
        SubType = wrappedModel.SubType switch
        {
            MemberChangeType.Approve => GroupMemberIncrease.SubType.Join,
            MemberChangeType.Invite => GroupMemberIncrease.SubType.Invite,
            _ => throw new ArgumentOutOfRangeException()
        };

        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string SubType { get; init; }

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.ChangedUser.Id.ToString();

    public string OperatorId => WrappedModel.Operator.Id.ToString();

    public GroupMemberChangeEventArgs WrappedModel { get; }
}
