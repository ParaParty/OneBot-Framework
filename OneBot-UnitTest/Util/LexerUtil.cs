using System.Collections.Generic;
using OneBot.CommandRoute.Lexer;

namespace OneBot_UnitTest.Util
{
    public static class LexerUtil
    {
        public static List<object> ExtraAllTokens(this CommandLexer self)
        {
            var ret = new List<object>();
            try
            {
                while (true)
                {
                    ret.Add(self.GetNextNotBlank());
                }
            }
            catch (ParserToTheEndException)
            {
                // ignored
            }

            return ret;
        }
    }
}
