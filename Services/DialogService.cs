using BizCardApp.Enums;
using BizCardApp.Enums.ValidationResults.Optional;
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
                      HandleLastNameErrors(validationOutcome) ??
                      HandleFullNameErrors(validationOutcome) ??
                      HandleCompanyErrors(validationOutcome) ??
                      HandleJobTitleErrors(validationOutcome) ??
                      HandlePhoneErrors(validationOutcome) ??
                      HandleEmailErrors(validationOutcome);

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

    private static string? HandleFullNameErrors(BusinessCardValidationOutcome validationOutcome)
    {
        if (validationOutcome.FullNameError is not null)
        {
            return validationOutcome.FullNameError switch
            {
                FullNameValidationResult.NotUnique => AppStrings.Dialogs.BusinessCard.Required.FullName.NotUnique,
                _ => null
            };
        }
        return null;
    }

    private static string? HandleCompanyErrors(BusinessCardValidationOutcome validationOutcome)
    {
        if (validationOutcome.CompanyError is not null)
        {
            return validationOutcome.CompanyError switch
            {
                CompanyValidationResult.TooShort => AppStrings.Dialogs.BusinessCard.Optional.Company.TooShort,
                CompanyValidationResult.TooLong => AppStrings.Dialogs.BusinessCard.Optional.Company.TooLong,
                CompanyValidationResult.InvalidCharacters => AppStrings.Dialogs.BusinessCard.Optional.Company.InvalidCharacters,
                _ => null
            };
        }
        return null;
    }

    private static string? HandleJobTitleErrors(BusinessCardValidationOutcome validationOutcome)
    {
        if (validationOutcome.JobTitleError is not null)
        {
            return validationOutcome.JobTitleError switch
            {
                JobTitleValidationResult.TooShort => AppStrings.Dialogs.BusinessCard.Optional.JobTitle.TooShort,
                JobTitleValidationResult.TooLong => AppStrings.Dialogs.BusinessCard.Optional.JobTitle.TooLong,
                JobTitleValidationResult.InvalidCharacters => AppStrings.Dialogs.BusinessCard.Optional.JobTitle.InvalidCharacters,
                _ => null
            };
        }
        return null;
    }

    private static string? HandlePhoneErrors(BusinessCardValidationOutcome validationOutcome)
    {
        if (validationOutcome.PhoneError is not null)
        {
            return validationOutcome.PhoneError switch
            {
                PhoneValidationResult.TooShort => AppStrings.Dialogs.BusinessCard.Optional.Phone.TooShort,
                PhoneValidationResult.TooLong => AppStrings.Dialogs.BusinessCard.Optional.Phone.TooLong,
                PhoneValidationResult.InvalidCharacters => AppStrings.Dialogs.BusinessCard.Optional.Phone.InvalidCharacters,
                PhoneValidationResult.InvalidFormat => AppStrings.Dialogs.BusinessCard.Optional.Phone.InvalidFormat,
                _ => null
            };
        }
        return null;
    }

    private static string? HandleEmailErrors(BusinessCardValidationOutcome validationOutcome)
    {
        if (validationOutcome.EmailError is not null)
        {
            return validationOutcome.EmailError switch
            {
                EmailValidationResult.TooLong => AppStrings.Dialogs.BusinessCard.Optional.Email.TooLong,
                EmailValidationResult.InvalidCharacters => AppStrings.Dialogs.BusinessCard.Optional.Email.InvalidCharacters,
                EmailValidationResult.InvalidFormat => AppStrings.Dialogs.BusinessCard.Optional.Email.InvalidFormat,
                _ => null
            };
        }
        return null;
    }
}