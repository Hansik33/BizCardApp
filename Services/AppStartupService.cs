using BizCardApp.Interfaces;
using BizCardApp.Views;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BizCardApp.Services;

public sealed class AppStartupService(MainWindow mainWindow,
                                      INavigationService navigationService,
                                      IBusinessCardService businessCardService)
{
    private bool _started;

    public async Task StartAsync()
    {
        if (_started) return;
        _started = true;

        mainWindow.Activate();
        navigationService.Initialize(mainWindow.MainContent);

        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        bool connected;
        try { connected = await businessCardService.CanConnectAsync(cancellationTokenSource.Token); }
        catch { connected = false; }

        Debug.WriteLine(connected ? "DB: OK" : "DB: FAIL");
    }
}