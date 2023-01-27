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
        switch (str[startPos])
        {
            case '#':
                return NextCommentToken();
            case '/' when StringNext(str, startPos) == '/':
                // 注释
                return NextCommentToken();
            // 普通字符串
            case '/':
                return NextStringToken();
            case '-':
            {
                if (StringNext(str, startPos) == '-')
                {
                    return NextFullFlagToken();
                }

                // 短 flag
                return NextShortenFlagToken();
            }
            case ' ':
            case '\n':
            case '\r':
            case '\t':
                return new CommandToken(new SimpleMessage(str[startPos]), TokenType.WhiteSpace, startSeg, startPos, startSeg, startPos + 1);
            default:
                return NextStringToken();
        }
    }

    private CommandToken NextShortenFlagToken()
    {
        throw new NotImplementedException();
    }

    private CommandToken NextFullFlagToken()
    {
        throw new NotImplementedException();
    }

    private CommandToken NextStringToken()
    {
        throw new NotImplementedException();
    }

    private CommandToken NextCommentToken()
    {
        var startSeg = _cntSegment;
        var startPos = _cntPosition;
        var end = _routeInfo.Message.MessageLength();
        var token = _routeInfo.Message.SubMessage(new Message.Index(startSeg, startPos));
        return new CommandToken(token, TokenType.Comment, startSeg, startPos, end.Segment, end.Position);
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
