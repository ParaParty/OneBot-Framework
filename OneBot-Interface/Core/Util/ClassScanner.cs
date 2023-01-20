using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneBot.Core.Util;

public static class ClassScanner
{
    public static IEnumerable<Type> ScanCurrentDomain(Func<Type, bool> predictor)
    {
        var dst = AppDomain.CurrentDomain.GetAssemblies();
        return Scan(dst, predictor);
    }

    public static IEnumerable<Type> Scan(IEnumerable<Assembly> assembly, Func<Type, bool> predictor)
    {
        var dst = AppDomain.CurrentDomain.GetAssemblies();

        var ret = new List<Type>();

        foreach (var item in dst)
        {
            ret.AddRange(Scan(item, predictor));
        }

        return ret;
    }

    public static IEnumerable<Type> Scan(Assembly assembly, Func<Type, bool> predictor)
    {
        return assembly.GetTypes().Where(predictor);
    }
}
