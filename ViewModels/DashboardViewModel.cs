using BizCardApp.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace BizCardApp.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
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

    public DashboardViewModel()
    {
        SaveChangesCommand = new RelayCommand(SaveChanges, () => SelectedBusinessCard is not null);
        AddBusinessCardCommand = new RelayCommand(AddBusinessCard);
        DeleteBusinessCardCommand = new RelayCommand(DeleteBusinessCard, () => SelectedBusinessCard is not null);
    }

    private void SaveChanges()
    {
        if (SelectedBusinessCard != null)
        {
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
        var latestBusinessCardForm = BusinessCards.LastOrDefault() ?? NewBusinessCardDraft;

        if (string.IsNullOrWhiteSpace(latestBusinessCardForm.FirstName)
            || string.IsNullOrWhiteSpace(latestBusinessCardForm.LastName))
            return;

        if (SelectedBusinessCard is null)
        {
            var businessCard = _newBusinessCardDraft;
            BusinessCards.Add(businessCard);
            SelectedBusinessCard = businessCard;

            _newBusinessCardDraft = new BusinessCardViewModel();

            OnPropertyChanged(nameof(BusinessCardForm));
        }
        else
        {
            var businessCard = new BusinessCardViewModel();
            BusinessCards.Add(businessCard);
            SelectedBusinessCard = businessCard;
        }
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
}