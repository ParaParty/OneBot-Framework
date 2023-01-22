using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace OneBot.UnitTest.ServiceProvider;

public class MockServiceProvider : IServiceProvider
{
    public ConcurrentDictionary<Type, object> ObjectPool { get; set; } = new ConcurrentDictionary<Type, object>();

    public void AddService<T>(T obj)
    {
        ObjectPool[typeof(T)] = obj!;
    }

    public object GetService(Type serviceType)
    {
        return ObjectPool[serviceType];
    }
}

public class MockServiceScope : IServiceScope
{
    public MockServiceScope(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    public void Dispose()
    {
    }

    public IServiceProvider ServiceProvider { get; init; }
}
