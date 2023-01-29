using System;
using OneBot.CommandRoute.Parser;

namespace OneBot.CommandRoute.Exceptions;

public class SyntaxErrorException : Exception
{
    internal SyntaxErrorException(CommandToken token, TokenType expectedTokenType): 
        base($"syntax error. Segment:{token.Start.Segment}, Position:{token.Start.Position}. {expectedTokenType} expected but {token.TokenType} accept")
    {
    }
    
    internal SyntaxErrorException(CommandToken token, string expectedTokenType):
        base($"syntax error. Segment:{token.Start.Segment}, Position:{token.Start.Position}. {expectedTokenType} expected but {token.TokenType} accept")
    {
    }
    
    internal SyntaxErrorException(CommandToken token, TokenType[] expectedTokenType):
        base($"syntax error. Segment:{token.Start.Segment}, Position:{token.Start.Position}. {expectedTokenType} expected but {token.TokenType} accept")
    {
    }
}
