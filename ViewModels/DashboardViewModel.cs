using BizCardApp.Enums.ValidationResults;
using BizCardApp.Helpers;
using BizCardApp.Interfaces;
using BizCardApp.Validators;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace BizCardApp.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
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

    public DashboardViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;

        SaveChangesCommand = new RelayCommand(SaveChanges, () => SelectedBusinessCard is not null);
        AddBusinessCardCommand = new RelayCommand(AddBusinessCard);
        DeleteBusinessCardCommand = new RelayCommand(DeleteBusinessCard, () => SelectedBusinessCard is not null);
    }

    private void SaveChanges()
    {
        if (SelectedBusinessCard is not null)
        {
            if (!ValidateAndHandleErrorsBeforeSave())
                return;
            else
                Debug.WriteLine($"Godność: {SelectedBusinessCard.FirstName} {SelectedBusinessCard.LastName}\r\n" +
                $"Firma: {SelectedBusinessCard.Company}\r\n" +
                $"Stanowisko: {SelectedBusinessCard.JobTitle}\r\n" +
                $"Telefon: {SelectedBusinessCard.Phone}\r\n" +
                $"E-mail: {SelectedBusinessCard.Email}\r\n" +
                $"Adres: {SelectedBusinessCard.Address}");
        }
    }

    private void AddBusinessCard()
    {
        if (!ValidateAndHandleErrorsBeforeAdd())
            return;

        if (SelectedBusinessCard is null)
            AddNewBusinessCard();
        else
            AddEmptyBusinessCard();
    }

    private void DeleteBusinessCard()
    {
        if (SelectedBusinessCard is null) return;

        var index = BusinessCards.IndexOf(SelectedBusinessCard);

        BusinessCards.Remove(SelectedBusinessCard);

        SelectedBusinessCard = BusinessCards.Count == 0
            ? null
            : BusinessCards[Math.Clamp(index - 1, 0, BusinessCards.Count - 1)];
    }

    private bool ValidateAndHandleErrorsBeforeSave()
    {
        if (SelectedBusinessCard is null)
            return false;

        var validationOutcome = BusinessCardValidator.Validate(SelectedBusinessCard, BusinessCards);

        if (validationOutcome.Result is BusinessCardValidationResult.Failure)
        {
            return false;
        }
        return true;
    }

    private bool ValidateAndHandleErrorsBeforeAdd()
    {
        var latestBusinessCardForm = BusinessCards.LastOrDefault() ?? NewBusinessCardDraft;
        if (SelectedBusinessCard is not null)
            SelectedBusinessCard = latestBusinessCardForm;

        var validationOutcome = BusinessCardValidator.Validate(latestBusinessCardForm);

        if (validationOutcome.Result is BusinessCardValidationResult.Failure)
        {
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