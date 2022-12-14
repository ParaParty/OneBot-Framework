namespace OneBot.CommandRoute.Model.Action;

public interface GroupMemberInfoRequest : GroupBasicRequest
{
    string UserId { get; }
    
}
