using System;
using System.Collections.Generic;

namespace OneBot.Core.Configuration;

public class PipelineBuilder
{
    private List<Type> _item = new List<Type>();

    public PipelineBuilder Use(Type t)
    {
        _item.Add(t);
        return this;
    }

    public List<Type> Build()
    {
        return _item;
    }
}
