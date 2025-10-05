using BizCardApp.Helpers;
using BizCardApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;

namespace BizCardApp.Services;

public class NavigationService(IServiceProvider serviceProvider) : INavigationService
{
    private ContentControl? _host;

    public void Initialize(ContentControl host) => _host = host;

    public void NavigateTo<TViewModel>() where TViewModel : class
    {
        if (_host == null)
            throw new InvalidOperationException("NavigationService not initialized.");

        var viewType = ViewLocator.ResolveViewType(typeof(TViewModel));
        var view = (UserControl)Activator.CreateInstance(viewType)!;
        view.DataContext = serviceProvider.GetRequiredService<TViewModel>();

        _host.Content = view;
    }
}