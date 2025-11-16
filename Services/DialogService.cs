using BizCardApp.Enums;
using BizCardApp.Enums.ValidationResults.Required;
using BizCardApp.Interfaces;
using BizCardApp.Resources;
using BizCardApp.Validators.Results;
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

    public async Task ShowErrorAsync(BusinessCardValidationOutcome validationOutcome)
    {
        var message = HandleFirstNameErrors(validationOutcome) ??
                      HandleLastNameErrors(validationOutcome);

        if (message is not null)
        {
            await ShowMessageAsync(message, DialogType.Error);
            return;
        }
        return;
    }

    private static string? HandleFirstNameErrors(BusinessCardValidationOutcome validationOutcome)
    {
        if (validationOutcome.FirstNameError is not null)
        {
            return validationOutcome.FirstNameError switch
            {
                FirstNameValidationResult.Empty => AppStrings.Dialogs.BusinessCard.Required.FirstName.Empty,
                FirstNameValidationResult.TooLong => AppStrings.Dialogs.BusinessCard.Required.FirstName.TooLong,
                FirstNameValidationResult.TooShort => AppStrings.Dialogs.BusinessCard.Required.FirstName.TooShort,
                FirstNameValidationResult.InvalidCharacters =>
                AppStrings.Dialogs.BusinessCard.Required.FirstName.InvalidCharacters,
                _ => null
            };
        }
        return null;
    }

    private static string? HandleLastNameErrors(BusinessCardValidationOutcome validationOutcome)
    {
        if (validationOutcome.LastNameError is not null)
        {
            return validationOutcome.LastNameError switch
            {
                LastNameValidationResult.Empty => AppStrings.Dialogs.BusinessCard.Required.LastName.Empty,
                LastNameValidationResult.TooLong => AppStrings.Dialogs.BusinessCard.Required.LastName.TooLong,
                LastNameValidationResult.TooShort => AppStrings.Dialogs.BusinessCard.Required.LastName.TooShort,
                LastNameValidationResult.InvalidCharacters =>
                AppStrings.Dialogs.BusinessCard.Required.LastName.InvalidCharacters,
                _ => null
            };
        }
        return null;
    }
}