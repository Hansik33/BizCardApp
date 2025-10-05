using BizCardApp.Interfaces;
using BizCardApp.Views;

namespace BizCardApp.Services;

public class AppStartupService(MainWindow mainWindow, INavigationService navigationService)
{
    private bool _started;

    public void Start()
    {
        if (_started) return;
        _started = true;

        mainWindow.Activate();
        navigationService.Initialize(mainWindow.MainContent);
    }
}