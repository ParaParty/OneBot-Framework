using System;
using OneBot.Platform.QQ.Event;
using OneBot.Provider.SoraProvider.Util;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraGroupRequest : AddGroupRequest, UnderlaySoraEvent<AddGroupRequestEventArgs>
{
    public SoraGroupRequest(AddGroupRequestEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;

        SubType = WrappedModel.SubType switch
        {
            GroupRequestType.Add => AddGroupRequest.SubType.Add,
            GroupRequestType.Invite => AddGroupRequest.SubType.Invite,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string SubType { get; }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string Comment => WrappedModel.Comment;

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string InvitorUserId => WrappedModel.InvitorUser.Id.ToString();

    public string RequestFlag => WrappedModel.RequestFlag;

    public AddGroupRequestEventArgs WrappedModel { get; }
}