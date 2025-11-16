using BizCardApp.Enums;
using BizCardApp.Validators.Results;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace BizCardApp.Interfaces;

public interface IDialogService
{
    void SetXamlRoot(XamlRoot root);

    Task ShowMessageAsync(string message, DialogType dialogType = DialogType.Info);
    Task<bool> ShowConfirmationAsync(string message);
    Task ShowInfoAsync();

    Task ShowErrorAsync(BusinessCardValidationOutcome validationOutcome);
}