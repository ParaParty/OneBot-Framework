using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Lexer;
using OneBot_UnitTest.Util;
using Sora.Entities;
using Sora.Entities.Segment;

namespace OneBot_UnitTest;

[TestClass]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class LexerTest
{

    [TestMethod]
    public void LexerTestForString()
    {
        List<(List<SoraSegment>, List<object>)> testCasts = new()
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
            (new (){"a","b c d"}, new (){"ab", "c", "d"}),
            (new (){"a ","        b        c                  d"}, new (){"a", "b", "c", "d"}),
            (new (){"a","b","c","d"}, new (){"abcd"}),
        };
            
        testCasts.ForEach((data) =>
        {
            var lexer = new CommandLexer(data.Item1);
            var tokens = lexer.ExtraAllTokens();
            AssertUtil.ArrayAreEqual(data.Item2, tokens);
            AssertUtil.ArrayAreEqual(lexer.ParsedArguments, tokens);
        });
    }
        
        
    [TestMethod]
    public void LexerForNormalMixedMessage()
    {
        List<(List<SoraSegment>, List<object>)> testCasts = new()
        {
            (new (){"mute ", SoraSegment.At(114514), " 1d"}, new (){"mute", SoraSegment.At(114514), "1d"}),
            (new (){"mute", SoraSegment.At(114514), "1d"}, new (){"mute", SoraSegment.At(114514), "1d"}),
        };
            
        testCasts.ForEach((data) =>
        {
            var lexer = new CommandLexer(data.Item1);
            var tokens = lexer.ExtraAllTokens();
            AssertUtil.ArrayAreEqual(data.Item2, tokens);
            AssertUtil.ArrayAreEqual(lexer.ParsedArguments, tokens);
        });
    }


    [TestMethod]
    public void LexerTestForQuotedString()
    {
        List<(List<SoraSegment>, List<object>)> testCasts = new()
        {
            (new() { "a \"b c\" d" }, new() { "a", new MessageBody { "b c" }, "d" }),
            (new() { "a \"b\n c\" d" }, new() { "a", new MessageBody { "b\n c" }, "d" }),
            (new() { "a \"b c\"" }, new() { "a", new MessageBody { "b c" } }),
            (new() { "\"a a\",\"" }, new() { new MessageBody { "a a" }, ",\"" }),
            (new() { "\"''''\" '\"\"\"\"'" }, new() { new MessageBody { "''''" }, new MessageBody { "\"\"\"\"" } }),
            (new() { "\"    ''''    \" '    \"\"\"\"    '" }, new() { new MessageBody { "    ''''    " }, new MessageBody { "    \"\"\"\"    " } }),
            (new() { "\"114514" }, new() { new MessageBody { "114514" } }),
            (new() { "'1919810" }, new() { new MessageBody { "1919810" } }),
            (new() { "\"\"\"\"" }, new() { new MessageBody { "\"" } }),
            (new() { "\"", "", "\"", "", "", "\"", "", "", "\"" }, new() { new MessageBody { "\"" } }),
        };

        testCasts.ForEach((data) =>
        {
            var lexer = new CommandLexer(data.Item1);
            var tokens = lexer.ExtraAllTokens();
            AssertUtil.ArrayAreEqual(data.Item2, tokens);
            AssertUtil.ArrayAreEqual(lexer.ParsedArguments, tokens);
        });
    }
        
    [TestMethod]
    public void LexerForNormalQuotedMixedMessage()
    {
        List<(List<SoraSegment>, List<object>)> testCasts = new()
        {
            (
                new() { "draw 喜报 \"恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。\"" }, 
                new() { "draw", "喜报", new MessageBody { "恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。" }}
            ),
            (
                new() { "draw 喜报 '恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。'" }, 
                new() { "draw", "喜报", new MessageBody { "恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。" }}
            ),
            (
                new()
                {
                    "draw 喜报 \"恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。\"'  ", SoraSegment.At(1919810), "  '"
                },
                new()
                {
                    "draw", "喜报", new MessageBody { "恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。" },
                    new MessageBody { "  ", SoraSegment.At(1919810), "  " }
                }
            ),
            (
                new() { "draw 喜报 \"恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。\"", SoraSegment.At(1919810) }, 
                new() { "draw", "喜报", new MessageBody { "恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。" }, SoraSegment.At(1919810)}
            ),
            (
                new() { "draw 喜报 \"恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。\"", "test" }, 
                new() { "draw", "喜报", new MessageBody { "恭喜 ", SoraSegment.At(114514), "获得群 Bug 最多奖。" }, "test"}
            ),
        };

        testCasts.ForEach((data) =>
        {
            var lexer = new CommandLexer(data.Item1);
            var tokens = lexer.ExtraAllTokens();
            AssertUtil.ArrayAreEqual(data.Item2, tokens);
            AssertUtil.ArrayAreEqual(lexer.ParsedArguments, tokens);
        });
    }
}