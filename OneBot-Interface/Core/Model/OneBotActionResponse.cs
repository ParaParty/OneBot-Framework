namespace OneBot.Core.Model;

public class OneBotActionResponse
{
    public OneBotActionResponse(string status, int retcode, object? data, string message)
    {
        Status = status;
        Retcode = retcode;
        Data = data;
        Message = message;
    }

    public string Status { get; }

    public int Retcode { get; }

    public object? Data { get; }

    public string Message { get; }
}
