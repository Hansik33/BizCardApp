using System;
using System.Collections.Generic;

namespace BizCardApp.Helpers;

public static class ViewLocator
{
    private static readonly Dictionary<Type, Type> Cache = [];

    public static Type ResolveViewType(Type viewModelType)
    {
        if (Cache.TryGetValue(viewModelType, out var cached))
            return cached;

        var vmFull = viewModelType.FullName!;
        var viewFull = vmFull
            .Replace(".ViewModels.", ".Views.")
            .Replace("ViewModel", "View");

        var viewType = viewModelType.Assembly.GetType(viewFull)
            ?? throw new InvalidOperationException($"View not found: {viewFull}");

        Cache[viewModelType] = viewType;
        return viewType;
    }
}