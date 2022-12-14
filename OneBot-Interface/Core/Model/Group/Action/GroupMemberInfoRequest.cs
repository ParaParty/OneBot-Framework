namespace OneBot.Core.Model.Group.Action;

public interface GroupMemberInfoRequest : GroupBasicRequest
{
    string UserId { get; }

}
