namespace OneBot.Core.Model;

public interface OneBotResponse
{
    string Status { get; }

    int Retcode { get; }

    string Message { get; }
}
