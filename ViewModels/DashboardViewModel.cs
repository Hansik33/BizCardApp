using BizCardApp.Helpers;
using System.Diagnostics;
using System.Windows.Input;

namespace BizCardApp.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    public ICommand SaveChangesCommand { get; }
    public ICommand AddBusinessCardCommand { get; }
    public ICommand DeleteBusinessCardCommand { get; }

    public DashboardViewModel()
    {
        SaveChangesCommand = new RelayCommand(SaveChanges);
        AddBusinessCardCommand = new RelayCommand(AddBusinessCard);
        DeleteBusinessCardCommand = new RelayCommand(DeleteBusinessCard);
    }

    private void SaveChanges() => Debug.WriteLine("SaveChanges");

    private void AddBusinessCard() => Debug.WriteLine("AddBusinessCard");

    private void DeleteBusinessCard() => Debug.WriteLine("DeleteBusinessCard");
}