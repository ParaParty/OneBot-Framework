using System;
using System.Collections.Generic;
using System.Data;
using OneBot.CommandRoute.Exceptions;
using SyntaxErrorException = OneBot.CommandRoute.Exceptions.SyntaxErrorException;

namespace OneBot.CommandRoute.Parser;

internal class CommandParser
{
    private readonly IReadOnlyList<CommandToken> _tokenList;

    private int _cntTokenIdx = 0;

    private CommandToken CntToken => _tokenList[_cntTokenIdx];

    private bool ReachEnd => _cntTokenIdx >= _tokenList.Count;

    private void WalkNextToken()
    {
        _cntTokenIdx++;
    }

    internal CommandParser(CommandLexer lexer)
    {
        var tokenList = new List<CommandToken>();
        try
        {
            while (true)
            {
                var token = lexer.NextToken();
                if (token.TokenType is TokenType.WhiteSpace or TokenType.Comment)
                {
                    continue;
                }
                tokenList.Add(token);
            }
        }
        catch (ReachEndException)
        {

        }

        _tokenList = tokenList;
    }

    internal AstNode.Command ParseCommand()
    {
        var commandSegment = ParseCommandSegments();
        var flags = ParseFlags();
        return new AstNode.Command(commandSegment, flags);
    }

    private AstNode.CommandSegments ParseCommandSegments()
    {
        var children = new List<AstNode.CommandSegment>();
        while (!ReachEnd && CntToken.TokenType is TokenType.Ident or TokenType.Value)
        {
            var ret = ParseCommandSegment();
            children.Add(ret);
        }
        return new AstNode.CommandSegments(children);
    }

    private AstNode.CommandSegment ParseCommandSegment()
    {
        var t = CntToken;
        WalkNextToken();
        return new AstNode.CommandSegment(t);
    }

    private AstNode.Flags ParseFlags()
    {
        var children = new List<AstNode.Flag>();
        while (!ReachEnd && CntToken.TokenType is TokenType.SingleDash or TokenType.DoubleDash)
        {
            var ret = CntToken.TokenType switch
            {
                TokenType.DoubleDash => ParseFlagFullName(),
                TokenType.SingleDash => ParseFlagShortenName(),
                _ => throw new ArgumentOutOfRangeException()
            };
            children.Add(ret);
        }
        if (!ReachEnd)
        {
            throw new SyntaxErrorException(CntToken, "EOF");
        }
        return new AstNode.Flags(children);
    }

    private AstNode.Flag ParseFlagShortenName()
    {
        // 来源唯一因此可以直接确定当前 token 为 single dash
        var singleDash = CntToken;
        WalkNextToken();

        var ident = CntToken;
        ident.AssertType(TokenType.Ident);
        WalkNextToken();

        return new AstNode.FlagShortenName(singleDash, ident);
    }

    private AstNode.FlagFullname ParseFlagFullName()
    {
        // 来源唯一因此可以直接确定当前 token 为 single dash
        var doubleDash = CntToken;
        WalkNextToken();

        var ident = CntToken;
        ident.AssertType(TokenType.Ident);
        WalkNextToken();

        if (CntToken.TokenType != TokenType.Equal)
        {
            return new AstNode.FlagFullname(doubleDash, ident);
        }

        var equal = CntToken;
        WalkNextToken();

        var value = CntToken;
        value.AssertType(TokenType.Ident, TokenType.Value);
        WalkNextToken();

        return new AstNode.FlagFullname(doubleDash, ident, equal, value);
    }
}
