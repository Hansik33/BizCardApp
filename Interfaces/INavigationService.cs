using Microsoft.UI.Xaml.Controls;

namespace BizCardApp.Interfaces;

public interface INavigationService
{
    void Initialize(ContentControl host);
    void NavigateTo<TViewModel>() where TViewModel : class;
    void GoToDashboard();
}