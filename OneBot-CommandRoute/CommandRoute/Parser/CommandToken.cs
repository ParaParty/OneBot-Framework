using OneBot.Core.Model.Message;

namespace OneBot.CommandRoute.Parser;

internal class CommandToken
{
    public CommandToken(Message token, TokenType tokenType, int startSegment, int startPosition, int endSegment, int endPosition)
    {
        Token = token;
        TokenType = tokenType;
        Start = new Message.Index(startSegment, startPosition);
        End = new Message.Index(endSegment, endPosition);
    }

    public Message Token { get; init; }

    public TokenType TokenType { get; init; }

    public Message.Index Start { get; init; }

    public Message.Index End { get; init; }
}