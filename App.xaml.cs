using BizCardApp.Interfaces;
using BizCardApp.Services;
using BizCardApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace BizCardApp;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<AppStartupService>();

        services.AddSingleton<INavigationService, NavigationService>();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args) =>
        Services.GetRequiredService<AppStartupService>().Start();
}