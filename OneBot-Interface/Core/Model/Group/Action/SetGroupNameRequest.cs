namespace OneBot.Core.Model.Group.Action;

public interface SetGroupNameRequest : GroupBasicRequest
{
    string GroupName { get; }
}
