using System;
using OneBot.CommandRoute.Configuration;
using OneBot.Core.Model.CommandRoute;
using OneBot.Core.Model.Message;
using OneBot.Core.Util;

namespace OneBot.CommandRoute.Parser;

internal class CommandLexer
{
    private readonly LexerConfiguration _lexerConfig;

    private readonly RouteInfo _routeInfo;

    private int _cntSegment;

    private int _cntPosition;

    internal CommandLexer(LexerConfiguration lexerConfig, RouteInfo routeInfo)
    {
        _lexerConfig = lexerConfig;
        _routeInfo = routeInfo;

        _cntSegment = routeInfo.StartSegment;
        _cntPosition = routeInfo.StartPosition;
    }

    internal CommandToken NextToken()
    {
        if (NowSegmentIsText)
        {
            return NextTextRelatedToken();
        }
        return NextNonTextRelatedToken();
    }

    private CommandToken NextNonTextRelatedToken()
    {
        var ret = new CommandToken(
            token: new SimpleMessage(_routeInfo.Message[_cntSegment]),
            tokenType: TokenType.Rich,
            startSegment: _cntSegment,
            startPosition: 0,
            endSegment: _cntSegment,
            endPosition: 0
        );
        _cntSegment++;
        _cntPosition = 0;
        return ret;
    }

    private CommandToken NextTextRelatedToken()
    {
        var startSeg = _cntSegment;
        var startPos = _cntPosition;

        var str = NowSegment.Get<string>("Message")!;
        if (str[startPos] == '/')
        {
            if (StringNext(str, startPos) == '/')
            {
                // 注释
                var end = _routeInfo.Message.MessageLength();
                var token = _routeInfo.Message.SubMessage(new Message.Index(startSeg, startPos));
                return new CommandToken(token, TokenType.Comment, startSeg, startPos, end.Segment, end.Position);
            }
            else
            {
                // 普通字符串
            }
        }
        else if (str[startPos] == '-')
        {
            if (StringNext(str, startPos) == '-')
            {
                // 完整 flag
            }
            else
            {
                // 短 flag
            }
        }
        else
        {
            // 普通字符串
        }
        throw new NotImplementedException();
    }

    private MessageSegmentRef NowSegment => _routeInfo.Message[_cntSegment];

    private bool NowSegmentIsText => NowSegment.GetSegmentType() == "text";

    private static char? StringNext(string str, int idx)
    {
        var len = str.Length;
        if (idx + 1 < len)
        {
            return str[idx + 1];
        }
        return null;
    }
}

internal enum TokenType
{
    String,

    Ident,

    Rich,

    DoubleDash,

    SingleDash,

    WhiteSpace,

    Comment,
}

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
