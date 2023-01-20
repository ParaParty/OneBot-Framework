using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OneBot.Core.Attributes;

namespace OneBot.Core.Util;

public class AttributeWalker
{

    private readonly Queue<Type> _pending = new Queue<Type>();

    private readonly Collection<Type> _mark = new Collection<Type>();

    private bool _started = false;

    public AttributeWalker(Type start)
    {
        _pending.Enqueue(start);
        _mark.Add(start);

    }

    public object? FindNext(Func<object, bool> predictor)
    {
        _started = true;
        while (_pending.TryPeek(out var type))
        {
            _pending.Dequeue();

            var attributes = type.GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                if (predictor(attribute))
                {
                    return attribute;
                }
            }

            foreach (var item in type.GetInterfaces())
            {
                if (_mark.Contains(item))
                {
                    continue;
                }
                _pending.Enqueue(item);
                _mark.Add(item);
            }
        }

        return null;
    }

    public T? FindNext<T>(Func<object, bool> predictor) where T : Attribute
    {
        var ret = FindNext(predictor);
        return ret as T;
    }

    public object? FindFirst(Func<object, bool> predictor)
    {
        if (_started)
        {
            throw new ArgumentException();
        }
        return FindNext(predictor);
    }

    public T? FindFirst<T>(Func<object, bool> predictor) where T : Attribute
    {
        var ret = FindFirst(predictor);
        return ret as T;
    }
}
