namespace OneBot.Core.Model.Group;

public interface GroupMessage
{
    string DetailType { get; }

    string MessageId { get; }

    Message.Message Message { get; }

    string AltMessage { get; }

    string GroupId { get; }

    string UserId { get; }
}
