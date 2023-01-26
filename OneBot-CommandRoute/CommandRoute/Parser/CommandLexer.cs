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
            token: new DefaultMessage(_routeInfo.Message[_cntSegment]),
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
        if (str[startSeg] == '/')
        {
            if (StringNext(str, startPos) == '/')
            {
                // 注释
            }
            else
            {
                // 普通字符串
            }
        } else if (str[startSeg] == '-')
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
    }

    private MessageSegmentRef NowSegment => _routeInfo.Message[_cntSegment];

    private bool NowSegmentIsText => NowSegment.GetSegmentType() == "text";

    private static char? StringNext(string str, int idx)
    {
        var len = str.Length;
        if (idx+1 < len)
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
        StartSegment = startSegment;
        StartPosition = startPosition;
        EndSegment = endSegment;
        EndPosition = endPosition;
    }

    public Message Token { get; init; }

    public TokenType TokenType { get; init; }

    public int StartSegment { get; init; }

    public int StartPosition { get; init; }

    public int EndSegment { get; init; }

    public int EndPosition { get; init; }
}
