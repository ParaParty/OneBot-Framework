namespace OneBot.CommandRoute.Model.Action;

public interface SetGroupNameRequest : GroupBasicRequest
{
    string GroupName { get; }
}
