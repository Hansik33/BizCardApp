using BizCardApp.Interfaces;
using BizCardApp.Resources;
using BizCardApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace BizCardApp.Views;

public sealed partial class MainWindow : Window
{
    private readonly DashboardViewModel _dashboard;
    private readonly IDialogService _dialogService;

    public MainWindow(IDialogService dialogService)
    {
        _dashboard = App.Services.GetRequiredService<DashboardViewModel>();
        _dialogService = dialogService;

        InitializeComponent();

        AppWindow.Closing += OnWindowClosing;
    }

    private async void InfoButton_Click(object _, RoutedEventArgs __) => await _dialogService.ShowInfoAsync();

    private async void OnWindowClosing(AppWindow sender, AppWindowClosingEventArgs args)
    {
        if (_dashboard.HasUnsavedChanges)
        {
            args.Cancel = true;

            if (await _dialogService.ShowConfirmationAsync(AppStrings.Dialogs.BusinessCard.ClosingWithoutSavingConfirmation))
                Close();
        }
    }
}