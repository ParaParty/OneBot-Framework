using System.Collections.Generic;

namespace OneBot.CommandRoute.Parser;

internal interface AstNode
{
    class Command : AstNode
    {
        public CommandSegments CommandSegments { get; }

        public Flags Flags { get; }

        public Command(CommandSegments commandSegments, Flags flags)
        {
            CommandSegments = commandSegments;
            Flags = flags;
        }

    }

    class CommandSegments : AstNode
    {
        public IReadOnlyList<CommandSegment> CommandSegment { get; }

        public CommandSegments(IReadOnlyList<CommandSegment> commandSegment)
        {
            CommandSegment = commandSegment;
        }
    }

    class CommandSegment : AstNode
    {
        public CommandToken CommandToken { get; }

        public CommandSegment(CommandToken commandToken)
        {
            CommandToken = commandToken;

        }
    }

    class Flags : AstNode
    {
        public List<Flag> Flag { get; }

        public Flags(List<Flag> flag)
        {
            Flag = flag;
        }
    }

    abstract class Flag : AstNode
    {

    }

    class FlagFullname : Flag
    {
        public CommandToken DoubleDash { get; }

        public CommandToken Key { get; }

        public CommandToken? Equal { get; }

        public CommandToken? Value { get; }

        public bool HasValue => Value != null;

        public FlagFullname(CommandToken doubleDash, CommandToken key)
        {
            DoubleDash = doubleDash;
            Key = key;
        }

        public FlagFullname(CommandToken doubleDash, CommandToken key, CommandToken equal, CommandToken value)
        {
            DoubleDash = doubleDash;
            Key = key;
            Equal = equal;
            Value = value;
        }
    }

    class FlagShortenName : Flag
    {
        public CommandToken SingleDash { get; }

        public CommandToken Ident { get; }

        public FlagShortenName(CommandToken singleDash, CommandToken ident)
        {
            SingleDash = singleDash;
            Ident = ident;
        }
    }
}
