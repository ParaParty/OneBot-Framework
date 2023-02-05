using System;
using System.Text.RegularExpressions;
using OneBot.CommandRoute.Configuration;
using OneBot.CommandRoute.Exceptions;
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

    public CommandLexer(LexerConfiguration lexerConfig, RouteInfo routeInfo)
    {
        _lexerConfig = lexerConfig;
        _routeInfo = routeInfo;

        _cntSegment = routeInfo.StartSegment;
        _cntPosition = routeInfo.StartPosition;
    }

    internal CommandToken NextToken()
    {
        if (ReachEnd)
        {
            throw new ReachEndException();
        }
        var ret = NowSegmentIsText ? NextTextRelatedToken() : NextNonTextRelatedToken();
        if (ret.Token.Count == 0)
        {
            throw new ApplicationException();
        }
        return ret;
    }

    private CommandToken NextNonTextRelatedToken()
    {
        var ret = new CommandToken(
            token: new SimpleMessage(_routeInfo.Message[_cntSegment]),
            tokenType: TokenType.Value,
            startSegment: _cntSegment,
            startPosition: 0,
            endSegment: _cntSegment + 1,
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
                    // 长 flag
                    WalkNextElement();
                    WalkNextElement();
                    return new CommandToken(new SimpleMessage("--"), TokenType.DoubleDash, startSeg, startPos, _cntSegment, _cntPosition);
                }

                // 短 flag
                WalkNextElement();
                return new CommandToken(new SimpleMessage('-'), TokenType.SingleDash, startSeg, startPos, _cntSegment, _cntPosition);
            }
            case '=':
            {
                WalkNextElement();
                return new CommandToken(new SimpleMessage('='), TokenType.Equal, startSeg, startPos, _cntSegment, _cntPosition);
            }
            case ' ':
            case '\n':
            case '\r':
            case '\t':
                var ch = str[startPos];
                WalkNextElement();
                var ret = new CommandToken(new SimpleMessage(ch), TokenType.WhiteSpace, startSeg, startPos, _cntSegment, _cntPosition);
                return ret;
            default:
                // 普通字符串
                return IdentOrLiteralValue();
        }
    }

    private static readonly Regex IdentRegex = new Regex("^[a-zA-Z\u0080-\uffff][a-zA-Z0-9\u0080-\uffff]*(-[a-zA-Z0-9\u0080-\uffff]+)*$");

    private CommandToken IdentOrLiteralValue(bool literalValue = false)
    {
        var ret = new Message.Builder();

        var startSeg = _cntSegment;
        var startPos = _cntPosition;

        var str = NowSegmentText;
        switch (str[startPos])
        {
            case '"':
                ret.Add('"');
                WalkNextElement();
                while (!ReachEnd)
                {
                    if (NowCharElement() != '"')
                    {
                        ret.Add(NowElement);
                        WalkNextElement();
                    }
                    else if (NextCharElement() == '"')
                    {
                        ret.Add("\"\"");
                        WalkNextElement();
                        WalkNextElement();
                    }
                    else
                    {
                        break;
                    }
                }
                ret.Add('"');
                WalkNextElement();
                break;
            case '\'':
                ret.Add('\'');
                WalkNextElement();
                while (!ReachEnd && NowCharElement() != '\'')
                {
                    ret.Add(NowElement);
                    WalkNextElement();
                }
                ret.Add('\'');
                WalkNextElement();
                break;
            default:
                while (!ReachEnd && NowCharElement() is not (' ' or '\n' or '\r' or '\t' or '='))
                {
                    ret.Add(NowElement);
                    WalkNextElement();
                }
                break;
        }

        var msg = ret.ToMessage();
        var type = TokenType.Value;
        if (!literalValue && msg.Count == 1 /* && msg[0].GetSegmentType() == "text" */)
        {
            var s = msg[0].GetText();
            if (s != null && IdentRegex.IsMatch(s))
            {
                type = TokenType.Ident;
            }
        }
        return new CommandToken(msg, type, startSeg, startPos, _cntSegment, _cntPosition);
    }

    private CommandToken NextCommentToken()
    {
        var startSeg = _cntSegment;
        var startPos = _cntPosition;
        var end = _routeInfo.Message.MessageLength();
        var token = _routeInfo.Message.SubMessage(new Message.Index(startSeg, startPos));
        _cntSegment = end.Segment;
        _cntPosition = end.Position;
        return new CommandToken(token, TokenType.Comment, startSeg, startPos, end.Segment, end.Position);
    }

    private IMessageSegment NowElement => NowSegmentIsText ? SimpleTextSegment.Build(NowSegmentText[_cntPosition]) : NowSegment;

    private Message.Index NextIndex()
    {
        var retSegment = _cntSegment;
        var retPosition = 0;
        if (NowSegmentIsText)
        {
            var str = NowSegmentText;
            retPosition = _cntPosition + 1;

            if (retPosition >= str.Length)
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

    private IMessageSegment NowSegment => _routeInfo.Message[_cntSegment];

    private bool NowSegmentIsText => NowSegment.GetSegmentType() == "text";

    private string NowSegmentText => NowSegment.GetText()!;

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
        var str = seg.GetText()!;
        return str[t.Position];
    }
}
