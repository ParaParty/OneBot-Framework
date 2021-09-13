using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Lexer;
using OneBot_UnitTest.Util;
using Sora.Entities.MessageElement;

namespace OneBot_UnitTest
{
    [TestClass]
    public class LexerTest
    {
        private List<(List<CQCode>, List<object>)> testCasts = new()
        {
            (new (){"a b c d"}, new (){"a", "b", "c", "d"}),
            (new (){@"a
b
c
d
e
"}, new (){"a", "b", "c", "d", "e"}),
            (new (){"a         b        c                  d"}, new (){"a", "b", "c", "d"}),
            (new (){@"a         b       c      




            d"}, new (){"a", "b", "c", "d"}),
            (new (){"测试\t\t测试"}, new (){"测试", "测试"}),

        };


        [TestMethod]
        public void LexerTestForString()
        {
            testCasts.ForEach((data) =>
            {
                var lexer = new CommandLexer(data.Item1);
                var tokens = lexer.ExtraAllTokens();
                AssertUtil.ArrayAreEqual(data.Item2, tokens);
            });
        }
    }
}
