using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using OneBot.Core.Model.Message;

namespace OneBot.CommandRoute.Parser;

public class ParsedCommand
{
    public class CommandSegment : IReadOnlyList<Message>
    {
        private readonly IImmutableList<Message> _commandSegment;

        public CommandSegment(IImmutableList<Message> commandSegment)
        {
            _commandSegment = commandSegment;
        }

        public IEnumerator<Message> GetEnumerator() => _commandSegment.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _commandSegment.GetEnumerator();

        public int Count => _commandSegment.Count;

        public Message this[int index] => _commandSegment[index];
    }

    public class FullNameFlagDictionary : IReadOnlyDictionary<string, Message?>
    {
        private readonly IImmutableDictionary<string, Message?> _flags;

        public FullNameFlagDictionary(IImmutableDictionary<string, Message?> flags)
        {
            _flags = flags;
        }

        public IEnumerator<KeyValuePair<string, Message?>> GetEnumerator() => _flags.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _flags.GetEnumerator();

        public int Count => _flags.Count;

        public bool ContainsKey(string key) => _flags.ContainsKey(key);

        public bool TryGetValue(string key, out Message? value)
        {
            return _flags.TryGetValue(key, out value);
        }

        public Message? this[string key] => _flags[key];

        public IEnumerable<string> Keys => _flags.Keys;

        public IEnumerable<Message?> Values => _flags.Values;
    }

    public class ShortenNameFlagSet : IReadOnlyCollection<string>
    {
        private readonly IImmutableSet<string> _flags;

        public ShortenNameFlagSet(IImmutableSet<string> flags)
        {
            _flags = flags;
        }

        public IEnumerator<string> GetEnumerator() => _flags.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _flags.GetEnumerator();

        public int Count => _flags.Count;

        public bool Contains(string s)
        {
            return _flags.Contains(s);
        }
    }

    public CommandSegment Command { get; }

    public FullNameFlagDictionary FullNameFlags { get; }

    public ShortenNameFlagSet ShortenFlags { get; }


    public ParsedCommand(IEnumerable<Message> command, IDictionary<string, Message?> fullFlags, Collection<string> shortenFlags)
    {
        Command = new CommandSegment(command.ToImmutableList());
        FullNameFlags = new FullNameFlagDictionary(fullFlags.ToImmutableDictionary());
        ShortenFlags = new ShortenNameFlagSet(shortenFlags.ToImmutableHashSet());
    }

}
