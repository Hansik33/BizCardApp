using BizCardApp.Enums;
using BizCardApp.Interfaces;
using BizCardApp.Resources;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace BizCardApp.Services;

public class DialogService : IDialogService
{
    private XamlRoot? _xamlRoot;
    private bool _isDialogOpen = false;

    public void SetXamlRoot(XamlRoot root) => _xamlRoot = root;

    public async Task ShowMessageAsync(string message, DialogType dialogType)
    {
        if (_xamlRoot is null || _isDialogOpen)
            return;

        _isDialogOpen = true;

        var title = dialogType switch
        {
            DialogType.Warning => AppStrings.Dialogs.TitleWarning,
            DialogType.Error => AppStrings.Dialogs.TitleError,
            DialogType.Success => AppStrings.Dialogs.TitleSuccess,
            _ => AppStrings.Dialogs.TitleInfo
        };

        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = _xamlRoot
        };

        await dialog.ShowAsync();
        _isDialogOpen = false;
    }

    public async Task<bool> ShowConfirmationAsync(string message)
    {
        if (_xamlRoot is null || _isDialogOpen)
            return false;

        _isDialogOpen = true;

        var dialog = new ContentDialog
        {
            Title = AppStrings.Dialogs.TitleWarning,
            Content = message,
            PrimaryButtonText = "Tak",
            CloseButtonText = "Nie",
            DefaultButton = ContentDialogButton.Close,
            XamlRoot = _xamlRoot
        };

        var result = await dialog.ShowAsync();
        _isDialogOpen = false;

        return result == ContentDialogResult.Primary;
    }

    public async Task ShowInfoAsync()
    {
        var dialog = new ContentDialog
        {
            Title = AppStrings.Dialogs.TitleInfo,
            Content = AppStrings.Dialogs.Info,
            PrimaryButtonText = "Otwórz GitHub",
            CloseButtonText = "Zamknij",
            DefaultButton = ContentDialogButton.Close,
            XamlRoot = _xamlRoot
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
            await Launcher.LaunchUriAsync(new Uri("https://github.com/Hansik33/BizCardApp"));
    }
}