namespace OneBot.Core.Model;

public class OneBotActionRequest
{
    public OneBotActionRequest(string action, object @params)
    {
        Action = action;
        Params = @params;
    }

    public string Action { get; }

    public object? Params { get; }
}
