using OneBot.Core.Model.Message;

namespace OneBot.Core.Interface;

public interface IMessageEvent
{
    Message GetMessage();
}
