using BizCardApp.Helpers;
using BizCardApp.Interfaces;
using BizCardApp.Resources;
using BizCardApp.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRT.Interop;

namespace BizCardApp.Services;

public sealed class AppStartupService(MainWindow mainWindow,
                                      IBusinessCardService businessCardService,
                                      IDialogService dialogService,
                                      INavigationService navigationService)
{
    private const string IconResourceName = "BizCardApp.Assets.App.ico";
    private const string IconCacheFileName = "App.ico";

    private bool _started;

    public async Task StartAsync()
    {
        if (_started)
            return;

        _started = true;

        SetAppWindowIcon(mainWindow);

        mainWindow.Activate();

        if (!await businessCardService.CanConnectAsync())
        {
            await dialogService.ShowMessageAsync(AppStrings.Dialogs.UnableToConnectDatabase, Enums.DialogType.Error);
            Process.GetCurrentProcess().Kill();
        }

        navigationService.Initialize(mainWindow.MainContent);
        navigationService.GoToDashboard();
    }

    private static void SetAppWindowIcon(MainWindow window)
    {
        var iconPath = ExtractIconToCache();

        var hwnd = WindowNative.GetWindowHandle(window);
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        appWindow.SetIcon(iconPath);
    }

    private static string ExtractIconToCache()
    {
        return EmbeddedIcon.EnsureExtracted(
            resourceName: IconResourceName,
            fileName: IconCacheFileName);
    }
}