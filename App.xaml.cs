using BizCardApp.Data;
using BizCardApp.Interfaces;
using BizCardApp.Services;
using BizCardApp.ViewModels;
using BizCardApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.IO;

namespace BizCardApp;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        InitializeComponent();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        Services = serviceCollection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var config = BuildConfigurationFromExePath("appsettings.json");
        services.AddSingleton<IConfiguration>(config);

        var connectionString = config.GetConnectionString("Default");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

        services.AddPooledDbContextFactory<AppDbContext>(options => options.UseMySql(connectionString, serverVersion));

        services.AddSingleton<IBusinessCardService, BusinessCardService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<AppStartupService>();

        services.AddSingleton<MainWindow>();
        services.AddSingleton<DashboardViewModel>();
    }

    private static IConfiguration BuildConfigurationFromExePath(string jsonFileName)
    {
        var exePath = Environment.ProcessPath;
        var exeDir = exePath is null
            ? AppContext.BaseDirectory
            : Path.GetDirectoryName(exePath) ?? AppContext.BaseDirectory;

        var configPath = Path.Combine(exeDir, jsonFileName);

        return new ConfigurationBuilder()
            .AddJsonFile(configPath, optional: true, reloadOnChange: true)
            .Build();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args) =>
        _ = Services.GetRequiredService<AppStartupService>().StartAsync();
}