using BizCardApp.Enums.ValidationResults;
using BizCardApp.Helpers;
using BizCardApp.Interfaces;
using BizCardApp.Mappers;
using BizCardApp.Resources;
using BizCardApp.Validators;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BizCardApp.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly IBusinessCardService _businessCardService;
    private readonly IDialogService _dialogService;

    public ObservableCollection<BusinessCardViewModel> BusinessCards { get; } = [];

    public BusinessCardViewModel BusinessCardForm => SelectedBusinessCard ?? NewBusinessCardDraft;

    private BusinessCardViewModel _newBusinessCardDraft = new();
    private BusinessCardViewModel NewBusinessCardDraft
    {
        get => _newBusinessCardDraft;
        set
        {
            if (SetProperty(ref _newBusinessCardDraft, value))
                OnPropertyChanged(nameof(BusinessCardForm));
        }
    }

    private BusinessCardViewModel? _selectedBusinessCard;
    public BusinessCardViewModel? SelectedBusinessCard
    {
        get => _selectedBusinessCard;
        set
        {
            if (SetProperty(ref _selectedBusinessCard, value))
            {
                OnPropertyChanged(nameof(BusinessCardForm));
                (SaveChangesCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteBusinessCardCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand SaveChangesCommand { get; }
    public ICommand AddBusinessCardCommand { get; }
    public ICommand DeleteBusinessCardCommand { get; }

    public DashboardViewModel(IBusinessCardService businessCardService,
                              IDialogService dialogService)
    {
        _businessCardService = businessCardService;
        _dialogService = dialogService;

        SaveChangesCommand = new RelayCommand(async () => await SaveChangesAsync(fromCommand: true),
            () => SelectedBusinessCard is not null);
        AddBusinessCardCommand = new RelayCommand(async () => await AddBusinessCardAsync());
        DeleteBusinessCardCommand = new RelayCommand(async () => await DeleteBusinessCardAsync(),
            () => SelectedBusinessCard is not null);
    }

    private async Task SaveChangesAsync(bool fromCommand = false)
    {
        if (SelectedBusinessCard is not null)
        {
            if (!await ValidateAndHandleErrorsBeforeSaveAsync())
                return;
            else
            {
                var entity = BusinessCardMapper.ToEntity(BusinessCardForm);
                var created = await _businessCardService.SaveBusinessCardAsync(entity);

                if (created is null)
                    return;

                if (fromCommand)
                    await _dialogService.ShowMessageAsync(AppStrings.Dialogs.BusinessCard.UpdateSuccess,
                                                          Enums.DialogType.Success);
            }
        }
    }

    private async Task AddBusinessCardAsync()
    {
        if (!await ValidateAndHandleErrorsBeforeAddAsync())
            return;

        if (SelectedBusinessCard is null)
        {
            AddNewBusinessCard();
            await SaveChangesAsync();
        }
        else
            AddEmptyBusinessCard();

        await _dialogService.ShowMessageAsync(AppStrings.Dialogs.BusinessCard.AddSuccess, Enums.DialogType.Success);
    }

    private async Task DeleteBusinessCardAsync()
    {
        if (SelectedBusinessCard is null)
            return;

        if (!await _dialogService.ShowConfirmationAsync(AppStrings.Dialogs.BusinessCard.DeleteConfirmation))
            return;

        var index = BusinessCards.IndexOf(SelectedBusinessCard);

        BusinessCards.Remove(SelectedBusinessCard);

        SelectedBusinessCard = BusinessCards.Count == 0
            ? null
            : BusinessCards[Math.Clamp(index - 1, 0, BusinessCards.Count - 1)];

        await _dialogService.ShowMessageAsync(AppStrings.Dialogs.BusinessCard.DeleteSuccess, Enums.DialogType.Success);
    }

    private async Task<bool> ValidateAndHandleErrorsBeforeSaveAsync()
    {
        if (SelectedBusinessCard is null)
            return false;

        var validationOutcome = BusinessCardValidator.Validate(SelectedBusinessCard, BusinessCards);

        if (validationOutcome.Result is BusinessCardValidationResult.Failure)
        {
            await _dialogService.ShowErrorAsync(validationOutcome);
            return false;
        }
        return true;
    }

    private async Task<bool> ValidateAndHandleErrorsBeforeAddAsync()
    {
        var latestBusinessCardForm = BusinessCards.LastOrDefault() ?? NewBusinessCardDraft;
        if (SelectedBusinessCard is not null)
            SelectedBusinessCard = latestBusinessCardForm;

        var validationOutcome = BusinessCardValidator.Validate(latestBusinessCardForm);

        if (validationOutcome.Result is BusinessCardValidationResult.Failure)
        {
            await _dialogService.ShowErrorAsync(validationOutcome);
            return false;
        }
        return true;
    }

    private void AddNewBusinessCard()
    {
        var businessCard = _newBusinessCardDraft;
        BusinessCards.Add(businessCard);
        SelectedBusinessCard = businessCard;
        _newBusinessCardDraft = new BusinessCardViewModel();
        OnPropertyChanged(nameof(BusinessCardForm));
    }

    private void AddEmptyBusinessCard()
    {
        var businessCard = new BusinessCardViewModel();
        BusinessCards.Add(businessCard);
        SelectedBusinessCard = businessCard;
    }
}