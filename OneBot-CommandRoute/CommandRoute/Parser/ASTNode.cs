using System;
using System.Collections.Generic;
using OneBot.Core.Model.Message;
using OneBot.Core.Util;

namespace OneBot.CommandRoute.Parser;

internal interface AstNode
{
    private static Message Unescape(CommandToken token)
    {
        if (token.Token.Count == 0)
        {
            return Message.EmptyMessage;
        }
        if (token.TokenType == TokenType.Ident)
        {
            return token.Token;
        }
        if (token.TokenType == TokenType.Value)
        {
            if (token.Token[0].TypeIsText())
            {
                var t = token.Token[0].GetText()!;
                if (t[0] == '"')
                {
                    return UnescapeDoubleQuote(token);
                }
                if (t[0] == '\'')
                {
                    return UnescapeSingleQuote(token);
                }
            }
            return token.Token;
        }
        throw new ArgumentException();
    }

    static Message UnescapeDoubleQuote(CommandToken token)
    {
        var msg = token.Token;
        if (msg.Count == 1)
        {
            return new Message.Builder().Add(msg[0].GetText()![1..^1].Replace("\"\"", "\"")).ToMessage();
        }

        var b = new Message.Builder();

        var first = msg[0];
        // if (!first.TypeIsText())
        b.Add(first.GetText()![1..].Replace("\"\"", "\""));

        for (int i = 0 + 1; i < msg.Count - 1; i++)
        {
            if (msg[i].TypeIsText())
            {
                var text = msg[i].GetText()!;
                b.Add(text.Replace("\"\"", "\""));
            }
            else
            {
                b.Add(msg[i]);
            }
        }

        var last = msg[^1];
        if (!last.TypeIsText())
        {
            throw new ArgumentException();
        }
        b.Add(last.GetText()![..^1].Replace("\"\"", "\""));
        return b.ToMessage();
    }

    static Message UnescapeSingleQuote(CommandToken token)
    {
        var msg = token.Token;
        if (msg.Count == 1)
        {
            return new Message.Builder().Add(msg[0].GetText()![1..^1]).ToMessage();
        }

        var b = new Message.Builder();

        var first = msg[0];
        // if (!first.TypeIsText())
        b.Add(first.GetText()![1..]);

        for (int i = 0 + 1; i < msg.Count - 1; i++)
        {
            b.Add(msg[i]);
        }

        var last = msg[^1];
        if (!last.TypeIsText())
        {
            throw new ArgumentException();
        }
        b.Add(last.GetText()![..^1]);
        return b.ToMessage();
    }

    internal class Command : AstNode
    {
        public CommandSegments CommandSegments { get; }

        public Flags Flags { get; }

        public Command(CommandSegments commandSegments, Flags flags)
        {
            CommandSegments = commandSegments;
            Flags = flags;
        }

    }

    internal class CommandSegments : AstNode
    {
        public IReadOnlyList<CommandSegment> CommandSegment { get; }

        public CommandSegments(IReadOnlyList<CommandSegment> commandSegment)
        {
            CommandSegment = commandSegment;
        }
    }

    internal class CommandSegment : AstNode
    {
        public CommandToken SegmentToken { get; }

        public Message Segment { get; }

        public CommandSegment(CommandToken segmentToken)
        {
            SegmentToken = segmentToken;
            Segment = Unescape(segmentToken);
        }
    }

    internal class Flags : AstNode
    {
        public List<Flag> Flag { get; }

        public Flags(List<Flag> flag)
        {
            Flag = flag;
        }
    }

    internal interface Flag : AstNode
    {
        public string FlagName { get; }
    }

    internal class FlagFullname : Flag
    {
        public CommandToken DoubleDashToken { get; }

        public CommandToken KeyToken { get; }

        public CommandToken? EqualToken { get; }

        public CommandToken? ValueToken { get; }

        public Message? Value { get; }

        public bool HasValue => Value != null;

        public string FlagName { get; }

        public FlagFullname(CommandToken doubleDashToken, CommandToken keyToken)
        {
            DoubleDashToken = doubleDashToken;
            KeyToken = keyToken;
            FlagName = keyToken.Token[0].GetText() ?? throw new ArgumentException();
        }

        public FlagFullname(CommandToken doubleDashToken, CommandToken keyToken, CommandToken equalToken, CommandToken valueToken)
        {
            DoubleDashToken = doubleDashToken;
            KeyToken = keyToken;
            EqualToken = equalToken;
            ValueToken = valueToken;
            FlagName = keyToken.Token[0].GetText() ?? throw new ArgumentException();
            Value = Unescape(ValueToken);
        }
    }

    internal class FlagShortenName : Flag
    {
        public CommandToken SingleDash { get; }

        public CommandToken Key { get; }

        public string FlagName { get; }

        public FlagShortenName(CommandToken singleDash, CommandToken key)
        {
            SingleDash = singleDash;
            Key = key;
            FlagName = key.Token[0].GetText() ?? throw new ArgumentException();

            "1234/5678/t".Split("/", 3);
        }
    }
}
