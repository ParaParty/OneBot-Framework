using System;
using System.Text.RegularExpressions;
using OneBot.CommandRoute.Configuration;
using OneBot.Core.Model.CommandRoute;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.SimpleMessageSegment;
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
            tokenType: TokenType.Value,
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

        var str = NowSegmentText;
        switch (str[startPos])
        {
            case '#':
                // 注释
                return NextCommentToken();
            case '/' when NextCharElement() == '/':
                // 注释
                return NextCommentToken();
            case '-':
            {
                if (NextCharElement() == '-')
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
                // 普通字符串
                return IdentOrLiteralValue();
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

    private static readonly Regex _identRegex = new Regex("^[a-zA-Z\u0080-\uffff][a-zA-Z0-9\u0080-\uffff]*$");

    private CommandToken IdentOrLiteralValue(bool literalValue = false)
    {
        var ret = new Message.Builder();

        var startSeg = _cntSegment;
        var startPos = _cntPosition;

        var str = NowSegmentText;
        switch (str[startPos])
        {
            case '"':
                throw new NotImplementedException();
                break;
            case '\'':
                WalkNextElement();
                while (!ReachEnd && NowCharElement() != '\'')
                {
                    ret.Add(NowElement);
                }
                break;
            default:
                while (!ReachEnd && NextCharElement() is not (' ' or '\n' or '\r' or '\t'))
                {
                    ret.Add(NowElement);
                }
                break;
        }

        var msg = ret.ToMessage();
        var type = TokenType.Value;
        if (!literalValue && msg.Count == 1 /* && msg[0].GetSegmentType() == "text" */)
        {
            var s = msg[0].Get<string>("Message");
            if (s != null && _identRegex.IsMatch(s))
            {
                type = TokenType.Ident;
            }
        }
        return new CommandToken(msg, type, startSeg, startPos, startSeg, startPos + 1);
    }

    private CommandToken NextCommentToken()
    {
        var startSeg = _cntSegment;
        var startPos = _cntPosition;
        var end = _routeInfo.Message.MessageLength();
        var token = _routeInfo.Message.SubMessage(new Message.Index(startSeg, startPos));
        return new CommandToken(token, TokenType.Comment, startSeg, startPos, end.Segment, end.Position);
    }

    private MessageSegmentRef NowElement => NowSegmentIsText ? SimpleTextSegment.Build(NowSegmentText[_cntPosition]) : NowSegment;

    private Message.Index NextIndex()
    {
        var retSegment = 0;
        var retPosition = 0;
        if (NowSegmentIsText)
        {
            var str = NowSegmentText;
            retPosition = _cntPosition + 1;

            if (_cntPosition >= str.Length)
            {
                retSegment++;
                retPosition = 0;
            }
        }
        else
        {
            retSegment++;
            retPosition = 0;
        }
        return new Message.Index(retSegment, retPosition);
    }

    private void WalkNextElement()
    {
        var t = NextIndex();
        _cntSegment = t.Segment;
        _cntPosition = t.Position;
    }

    private MessageSegmentRef NowSegment => _routeInfo.Message[_cntSegment];

    private bool NowSegmentIsText => NowSegment.GetSegmentType() == "text";

    private string NowSegmentText => NowSegment.Get<string>("Message")!;

    private bool ReachEnd => _cntSegment >= _routeInfo.Message.Count;

    private char? NowCharElement()
    {
        if (!NowSegmentIsText)
        {
            return null;
        }
        return NowSegmentText[_cntPosition];
    }

    private char? NextCharElement()
    {
        var t = NextIndex();
        if (t.Segment >= _routeInfo.Message.Count)
        {
            return null;
        }
        var seg = _routeInfo.Message[t.Segment];
        if (seg.GetSegmentType() != "text")
        {
            return null;
        }
        var str = seg.Get<string>("Message")!;
        return str[t.Position];
    }
}
