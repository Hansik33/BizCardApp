using BizCardApp.Enums.ValidationResults;
using BizCardApp.Helpers;
using BizCardApp.Interfaces;
using BizCardApp.Mappers;
using BizCardApp.Resources;
using BizCardApp.Validators;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BizCardApp.ViewModels;

public partial class DashboardViewModel : BaseViewModel, IAsyncInitializable
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
                (AddBusinessCardCommand as RelayCommand)?.RaiseCanExecuteChanged();

                if (_selectedBusinessCard != null)
                    _selectedBusinessCard.PropertyChanged += SelectedBusinessCard_PropertyChanged;
            }
        }
    }

    private bool _hasUnsavedChanges;
    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        private set => SetProperty(ref _hasUnsavedChanges, value);
    }

    public ICommand SaveChangesCommand { get; }
    public ICommand AddBusinessCardCommand { get; }
    public ICommand DeleteBusinessCardCommand { get; }
    public ICommand ClearBusinessCardCommand { get; }

    public DashboardViewModel(IBusinessCardService businessCardService,
                              IDialogService dialogService)
    {
        _businessCardService = businessCardService;
        _dialogService = dialogService;

        SaveChangesCommand = new RelayCommand(
            async () => await SaveChangesAsync(fromCommand: true),
            () => SelectedBusinessCard is not null && SelectedBusinessCard.IsDirty);

        AddBusinessCardCommand = new RelayCommand(
            async () => await AddBusinessCardAsync(),
            () => !HasUnsavedChanges
        );

        DeleteBusinessCardCommand = new RelayCommand(async () => await DeleteBusinessCardAsync(),
            () => SelectedBusinessCard is not null);

        ClearBusinessCardCommand = new RelayCommand(() => ClearBusinessCard(),
            () => SelectedBusinessCard is not null && SelectedBusinessCard.IsDirty);

        BusinessCards.CollectionChanged += BusinessCards_CollectionChanged;
    }

    public async Task InitializeAsync() => await LoadBusinessCardsAsync();

    private async Task LoadBusinessCardsAsync()
    {
        var entities = await _businessCardService.GetAllBusinessCardsAsync();

        BusinessCards.Clear();

        foreach (var entity in entities)
        {
            var viewModel = BusinessCardMapper.ToViewModel(entity);
            viewModel.TakeSnapshot();
            BusinessCards.Add(viewModel);
        }

        foreach (var businessCard in BusinessCards)
            businessCard.PropertyChanged += AnyBusinessCard_PropertyChanged;

        UpdateHasUnsavedChanges();

        SelectedBusinessCard = BusinessCards.FirstOrDefault();
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

                SelectedBusinessCard.Id = created.Id;
                SelectedBusinessCard.TakeSnapshot();
                (SaveChangesCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (AddBusinessCardCommand as RelayCommand)?.RaiseCanExecuteChanged();

                UpdateHasUnsavedChanges();

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

        UpdateHasUnsavedChanges();
    }

    private async Task DeleteBusinessCardAsync()
    {
        if (SelectedBusinessCard is null)
            return;

        if (!await _dialogService.ShowConfirmationAsync(AppStrings.Dialogs.BusinessCard.DeleteConfirmation))
            return;

        if (SelectedBusinessCard.Id > 0)
        {
            var deleted = await _businessCardService.DeleteBusinessCardAsync(SelectedBusinessCard.Id);

            if (!deleted)
                return;
        }

        DeleteBusinessCard();

        await _dialogService.ShowMessageAsync(
            AppStrings.Dialogs.BusinessCard.DeleteSuccess,
            Enums.DialogType.Success);

        UpdateHasUnsavedChanges();
    }

    private void ClearBusinessCard()
    {
        if (SelectedBusinessCard is null)
            return;

        SelectedBusinessCard.Clear();
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

        businessCard.TakeSnapshot();
        BusinessCards.Add(businessCard);

        businessCard.PropertyChanged += AnyBusinessCard_PropertyChanged;

        SelectedBusinessCard = businessCard;
        _newBusinessCardDraft = new BusinessCardViewModel();

        OnPropertyChanged(nameof(BusinessCardForm));
        UpdateHasUnsavedChanges();
    }

    private void AddEmptyBusinessCard()
    {
        var businessCard = new BusinessCardViewModel();

        businessCard.TakeSnapshot();
        BusinessCards.Add(businessCard);

        businessCard.PropertyChanged += AnyBusinessCard_PropertyChanged;

        SelectedBusinessCard = businessCard;

        UpdateHasUnsavedChanges();
    }

    private void DeleteBusinessCard()
    {
        if (SelectedBusinessCard is null)
            return;

        var cardToRemove = SelectedBusinessCard;
        var index = BusinessCards.IndexOf(cardToRemove);

        BusinessCards.Remove(cardToRemove);
        cardToRemove.PropertyChanged -= AnyBusinessCard_PropertyChanged;

        SelectedBusinessCard = BusinessCards.Count == 0
            ? null
            : BusinessCards[Math.Clamp(index - 1, 0, BusinessCards.Count - 1)];

        UpdateHasUnsavedChanges();
    }

    private void SelectedBusinessCard_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BusinessCardViewModel.IsDirty))
        {
            (SaveChangesCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ClearBusinessCardCommand as RelayCommand)?.RaiseCanExecuteChanged();
            UpdateHasUnsavedChanges();
        }
    }

    private void AnyBusinessCard_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BusinessCardViewModel.IsDirty))
        {
            (AddBusinessCardCommand as RelayCommand)?.RaiseCanExecuteChanged();
            UpdateHasUnsavedChanges();
        }
    }

    private void BusinessCards_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is BusinessCardViewModel businessCard)
                    businessCard.PropertyChanged -= AnyBusinessCard_PropertyChanged;
            }
        }

        if (e.NewItems is not null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is BusinessCardViewModel businessCard)
                    businessCard.PropertyChanged += AnyBusinessCard_PropertyChanged;
            }
        }

        (AddBusinessCardCommand as RelayCommand)?.RaiseCanExecuteChanged();
        UpdateHasUnsavedChanges();
    }

    private void UpdateHasUnsavedChanges()
    {
        HasUnsavedChanges = BusinessCards.Any(businessCard => businessCard.IsDirty);
    }
}