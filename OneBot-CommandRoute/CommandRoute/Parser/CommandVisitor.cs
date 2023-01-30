using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OneBot.Core.Model.Message;

namespace OneBot.CommandRoute.Parser;

internal class CommandVisitor
{
    internal CommandVisitor()
    {

    }

    internal ParsedCommand VisitCommand(AstNode.Command command)
    {
        var segment = command.CommandSegments.CommandSegment.Select(s => s.Segment);
        var fullFlags = new Dictionary<string, Message?>();
        var shortenFlags = new Collection<string>();
        foreach (var flag in command.Flags.Flag)
        {
            if (flag is AstNode.FlagFullname f)
            {
                fullFlags[f.FlagName] = f.Value;
            } else if (flag is AstNode.FlagShortenName s)
            {
                shortenFlags.Add(s.FlagName);
            }
        }
        return new ParsedCommand(segment, fullFlags, shortenFlags);
    }
}
