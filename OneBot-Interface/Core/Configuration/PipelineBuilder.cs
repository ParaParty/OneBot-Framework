using System;
using System.Collections.Generic;
using OneBot.Core.Interface;

namespace OneBot.Core.Configuration;

public class PipelineBuilder
{
    private List<Type> _item = new List<Type>();

    public PipelineBuilder Use(Type t)
    {
        if (!t.IsAssignableTo(typeof(IOneBotMiddleware)))
        {
            throw new ArgumentException("middleware registered in pipeline must be a subtype of IOneBotMiddleware");
        }
        _item.Add(t);
        return this;
    }

    public List<Type> Build()
    {
        return _item;
    }
}
