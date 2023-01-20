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

public class SoraFileUpload : FileUpload, UnderlaySoraEvent<FileUploadEventArgs>
{
    public SoraFileUpload(FileUploadEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string GroupId => WrappedModel.SourceGroup.Id.ToString();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string FileId => WrappedModel.FileInfo.FileId;

    public string FileName => WrappedModel.FileInfo.Name;

    public long FileSize => WrappedModel.FileInfo.Size;

    public string Busid => WrappedModel.FileInfo.Busid.ToString();

    public FileUploadEventArgs WrappedModel { get; }
}

public class SoraFriendRequest : FriendRequest, UnderlaySoraEvent<FriendRequestEventArgs>
{
    public SoraFriendRequest(FriendRequestEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string UserId => WrappedModel.Sender.Id.ToString();

    public string Comment => WrappedModel.Comment;

    public string RequestFlag => WrappedModel.RequestFlag;

    public FriendRequestEventArgs WrappedModel { get; }
}

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
