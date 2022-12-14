namespace OneBot.Core.Model.Channel.Action;

public interface GetChannelMemberListResponse : ChannelActionBasicRequest
{
    string ChannelId { get; }
}
