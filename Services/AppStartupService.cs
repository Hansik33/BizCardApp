using BizCardApp.Interfaces;
using BizCardApp.Resources;
using BizCardApp.Views;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BizCardApp.Services;

public sealed class AppStartupService(MainWindow mainWindow,
                                      IBusinessCardService businessCardService,
                                      IDialogService dialogService,
                                      INavigationService navigationService)
{
    private bool _started;

    public async Task StartAsync()
    {
        if (_started)
            return;

        _started = true;

        mainWindow.Activate();

        if (!await businessCardService.CanConnectAsync())
        {
            await dialogService.ShowMessageAsync(AppStrings.Dialogs.UnableToConnectDatabase, Enums.DialogType.Error);
            Process.GetCurrentProcess().Kill();
        }

        navigationService.Initialize(mainWindow.MainContent);
        navigationService.GoToDashboard();
    }
}