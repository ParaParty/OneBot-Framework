using OneBot.Core.Util;

namespace OneBot.Core.Interface;

public interface IOneBotNamingStrategy
{
    PropertyNamingStrategy Strategy { get; }
}
